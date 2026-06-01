using canbecheaperAPI.DTO;
using canbecheaperAPI.DTO.ProductPrice;
using canbecheaperAPI.DTO.Type;
using canbecheaperAPI.DTO.User;
using canbecheaperAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.VisualBasic;

namespace canbecheaperAPI.Endpoints
{
    public static class TypeEndpoints
    {

        public static void MapTypeEndpoints(this WebApplication app)
        {

            var group = app.MapGroup("/types")
                .WithTags("Types")
                .WithOpenApi();

            group.MapGet("/", async (DbOliwia230Context db) =>
            {
                var allTypes = await db.CheaperTypes
                .AsNoTracking()
                .Select(u => new TypeResponse(u.Id, u.Name, u.CreatedAt, u.UserId))
                .ToListAsync();

                return Results.Ok(allTypes);
            })
            .WithSummary("Get all types");


            group.MapGet("/{userId}", async (int userId, DbOliwia230Context db) =>
            {
                var types = await db.CheaperTypes
                .AsNoTracking()
                .Where(u => u.UserId == userId)
                .Select(u => new TypeResponse(u.Id, u.Name, u.CreatedAt, u.UserId))
                .ToListAsync();

                return Results.Ok(types);
            })
             .WithSummary("Get types by userId");

            group.MapPost("/save/{userId}", async (TypeRequest request, DbOliwia230Context db) =>
            {


                using var trans = await db.Database.BeginTransactionAsync();
                try
                {
                    CheaperType newType = new CheaperType
                    {
                        Name = request.name,
                        UserId = request.userId,
                        CreatedAt = DateTime.UtcNow
                    };
                    db.CheaperTypes.Add(newType);


                    await db.SaveChangesAsync();
                    await trans.CommitAsync();


                    return Results.Created();

                }
                catch (Exception ex)
                {

                    return Results.Problem($"Błąd podczas tworzenia: {ex.Message}");
                }
                finally
                {
                    await trans.RollbackAsync();
                }

            }).WithSummary("Create type");

            group.MapPost("/update/{id}/{name}/{userId}", async (int id, string name, int userId, DbOliwia230Context db) =>
            {

                try
                {


                    var typeToEdit = await db.CheaperTypes.FindAsync(id);
                    if (typeToEdit == null) return Results.NotFound(new { message = "Brak typu" });
                    if(typeToEdit.UserId != userId) return Results.NotFound(new { message = "Błędny użytkownik" });

                    typeToEdit.Name = name;


                    await db.SaveChangesAsync();


                    return Results.Created();

                }
                catch (Exception ex)
                {

                    return Results.Problem($"Błąd podczas tworzenia: {ex.Message}");
                }

            }).WithSummary("Create type"); ;

            group.MapDelete("delete/{id}/{userId}", async (int id, int userId, DbOliwia230Context db) =>
            {

                try
                {
                    CheaperType cheaperTypes = await db.CheaperTypes.FindAsync(id);
                    if (cheaperTypes == null) return Results.NotFound(new { message = "Brak typu" });
                    if (cheaperTypes.UserId == userId) return Results.NotFound(new { message = "Bład użytkownika" });

                    db.CheaperTypes.Remove(cheaperTypes);

                    await db.SaveChangesAsync();
                    return Results.NoContent();
                }
                catch (Exception ex)
                {

                    return Results.Problem($"Błąd podczas tworzenia: {ex.Message}");
                }

            }).WithSummary("Delete type");


        }
    }
}
