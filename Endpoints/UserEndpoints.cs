using canbecheaperAPI.DTO;
using canbecheaperAPI.Models;
using canbecheaperAPI.Utility;
using Microsoft.EntityFrameworkCore;

namespace canbecheaperAPI.Endpoints
{
    public static class UserEndpoints
    {

        public static void MapUserEndpoints(this WebApplication app)
        {

            var group = app.MapGroup("/api/users")
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


            group.MapPost("/", async (CreateUserRequest request, DbOliwia230Context db) =>
                {
                    var userToCheck = await db.CheaperUsers.FirstOrDefaultAsync(u => u.Email == request.email);


                    if (userToCheck != null && userToCheck.EmailConfirmed == false && userToCheck.EmailCodeExpiresAt < DateTime.UtcNow)
                        return Results.Conflict(new { message = "Z tego adresu mailowego została już podjęta próba rejstracji. Spróbuj później."});

                    if (userToCheck != null && userToCheck.EmailConfirmed == true)
                        return Results.Conflict(new { message = "Email jest już zajęty" });

                    int confirmCode = CodeGenerator.Generate();


                    if (userToCheck is not null)
                    {
                        userToCheck.EmailCodeExpiresAt = DateTime.UtcNow.AddMinutes(15);
                        userToCheck.EmailCode = confirmCode;

                        await db.SaveChangesAsync();
                        return Results.Ok(new { messages = "Wysłano nowy kod weryfikacyjny" }); 
                       
                    }

                    CheaperUser user = new CheaperUser
                    {
                        Name = request.name,
                        Email = request.email,
                        Password = request.password,
                        EmailConfirmed = false,
                        EmailCodeAttempts = 0,
                        EmailCodeExpiresAt = DateTime.UtcNow.AddMinutes(15),
                        EmailCode = confirmCode

                    };

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


            group.MapGet("/{id}", async (int id, DbOliwia230Context db) =>
            {
                var user = await db.CheaperUsers.FindAsync(id);
                return user is null ? Results.NotFound() : Results.Ok(user);

            })
            .WithName("GetUserById")
            .ExcludeFromDescription();

            group.MapPost("/", async (ConfirmCodeRequest request, DbOliwia230Context db) =>
            {
                CheaperUser userToConfirm = await db.CheaperUsers.FirstOrDefaultAsync(u => u.Email == request.email);





            });
        }
    }
}
