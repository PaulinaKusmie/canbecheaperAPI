using System;
using canbecheaperAPI.DTO;
using canbecheaperAPI.DTO.User;
using canbecheaperAPI.Models;
using canbecheaperAPI.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace canbecheaperAPI.Endpoints
{
    public static class UserEndpoints
    {

        public static void MapUserEndpoints(this WebApplication app)
        {

            var group = app.MapGroup("/users")
                .WithTags("Users")
                .WithOpenApi();


            group.MapGet("/", async (DbOliwia230Context db) =>
            {
                var users = await db.CheaperUsers
                .AsNoTracking()
                .Select(u => new UserResponse(u.Id, u.Name))
                .ToListAsync();

                return Results.Ok(users);
            })
             .WithSummary("Get all users");


            group.MapGet("/{id}", async (int id, DbOliwia230Context db) =>
            {
                var user = await db.CheaperUsers
                .AsNoTracking()
                .Where(u => u.Id == id)
                .Select(u => new UserResponse(u.Id, u.Name))
                .FirstOrDefaultAsync();

                return user is not null ? Results.Ok(user) : Results.NotFound(new { message = "Użytkownik nie istnieje" });
            })
             .WithSummary("Get user by id");


            group.MapPost("/login", async (LoginRequest loginReguest, DbOliwia230Context db) =>
            {
                CheaperUser userToLogin = await db.CheaperUsers.FirstOrDefaultAsync(u => u.Email == loginReguest.email && u.EmailConfirmed == true);

                if (userToLogin == null)
                    return Results.NotFound(new { message = "Użytkownik nie istnieje" });

                bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(loginReguest.password, userToLogin.Password);

                if (isPasswordCorrect)
                {
                    UserResponse userResponse = new UserResponse(userToLogin.Id, userToLogin.Name);


                    Console.WriteLine(userResponse.ToString());

                    return Results.Ok( userResponse);
                }
                return Results.Unauthorized();
            });

            
            group.MapPost("/register", async ([FromBody] RegisterRequest request, DbOliwia230Context db, MailService mailService) =>
                {
                    var userToCheck = await db.CheaperUsers.FirstOrDefaultAsync(u => u.Email == request.Email);


                    if (userToCheck != null && userToCheck.EmailConfirmed == false && userToCheck.EmailCodeExpiresAt < DateTime.UtcNow)
                        return Results.Conflict(new { message = "Z tego adresu mailowego została już podjęta próba rejstracji. Spróbuj później."});

                    if (userToCheck != null && userToCheck.EmailConfirmed == true)
                        return Results.Conflict(new { message = "Email jest już zajęty" });

                    int confirmCode = CodeGenerator.Generate();

                   string result = mailService.Send(request.Email, confirmCode);
                   if(result != string.Empty)
                        return Results.Conflict(new { message = result });

                    if (userToCheck is not null)
                    {
                        userToCheck.EmailCodeExpiresAt = DateTime.UtcNow.AddMinutes(15);
                        userToCheck.EmailCode = confirmCode;
                        userToCheck.EmailCodeAttempts = 0;
                        await db.SaveChangesAsync();
                        return Results.Ok(new { messages = "Wysłano nowy kod weryfikacyjny" }); 
                       
                    }

                    CheaperUser user = new CheaperUser
                    {
                        Name = request.Name,
                        Email = request.Email,
                        Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                        EmailConfirmed = false,
                        EmailCodeAttempts = 0,
                        EmailCodeExpiresAt = DateTime.UtcNow.AddMinutes(15),
                        EmailCode = confirmCode

                    };
                    ///DOPSIZ TWORZENIE JEDNOSTEK :)
                    db.CheaperUsers.Add(user);
                    await db.SaveChangesAsync();

                    return Results.Ok(new { message = "Sprawdź email." });
                })
                .WithName("Creeate User")
                .WithSummary("Create new user");

            group.MapDelete("/$id", async (int id, DbOliwia230Context db) =>
            {
                CheaperUser cheaperUserToDelete = await db.CheaperUsers.FindAsync(id);

                if (cheaperUserToDelete is null)
                    return Results.NotFound(new { message = "Brak użytkownika" });

                db.CheaperUsers.Remove(cheaperUserToDelete);
                await db.SaveChangesAsync();

                return Results.Ok();

            })
             .WithName("Delete User")
             .WithSummary("Delete new user");


            //group.MapGet("/{id}", async (int id, DbOliwia230Context db) =>
            //{
            //    var user = await db.CheaperUsers.FindAsync(id);
            //    return user is null ? Results.NotFound() : Results.Ok(user);

            //})
            //.WithName("GetUserById")
            //.ExcludeFromDescription();

            group.MapPost("/confirmCode", async (ConfirmCodeRequest request, DbOliwia230Context db) =>
            {
                CheaperUser userToConfirm = await db.CheaperUsers.FirstOrDefaultAsync(u => u.Email == request.email);


                if (userToConfirm is null) return Results.NotFound(new { message = "Brak użytkownika" });

                if (userToConfirm.EmailConfirmed) return Results.Conflict(new { message = "Email został już potwierdzony" });

                if (userToConfirm.EmailCodeExpiresAt < DateTime.UtcNow) return Results.Conflict(new { message = "Kod wygasł, spróbuj ponownie" });

                if (userToConfirm.EmailCodeAttempts > 3) return Results.Conflict(new { message = "Przekroczono limit prób" });


                if (request.code != userToConfirm.EmailCode) {

                    userToConfirm.EmailCodeAttempts += 1;
                    await db.SaveChangesAsync();
                    return Results.Conflict(new { message = "Email został potwierdzony" });
                }

                userToConfirm.EmailConfirmed = true;
                userToConfirm.EmailCode = null;
                userToConfirm.EmailCodeExpiresAt = null;
                userToConfirm.EmailCodeAttempts = 0;
                await db.SaveChangesAsync();
                return Results.Ok(new { message = "Email został potwierdzony" });

            }).WithName("ConfirmCode")
              .WithSummary("Confirm email with code");

        }
    }
}
