using canbecheaperAPI.DTO.Product;
using canbecheaperAPI.DTO.Unit;
using canbecheaperAPI.DTO.User;
using canbecheaperAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace canbecheaperAPI.Endpoints
{
    public static class UnitEndpoints
    {

        public static void MapUnitEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/units")
              .WithTags("Units")
              .WithOpenApi();

            group.MapGet("/{userId}", async (int userId, DbOliwia230Context db) =>
            {
                var unit = await db.CheaperUnits
                .AsNoTracking()
                .Where(u => u.UserId == userId)
                .Select(u => new UnitResponse(u.Id, u.UserId, u.WeightUnit, u.LengthUnit, u.VolumeUnit, u.PieceUnit))
                .FirstOrDefaultAsync();

                return unit is not null ? Results.Ok(unit) : Results.NotFound(new { message = "Jednoski nie istnieją" });
            })
            .WithSummary("Get unit by id");

            group.MapPatch("/update", async (UnitRequest request, DbOliwia230Context db) =>
            {

                var unitToEdit = await db.CheaperUnits.FirstOrDefaultAsync(p => p.UserId == request.userId);
                if (unitToEdit is null)
                    return Results.Conflict(new { message = "Konfiguracja nie istnieje" });



                unitToEdit.WeightUnit = request.weightUnit;
                unitToEdit.LengthUnit = request.lengthUnit;
                unitToEdit.VolumeUnit = request.volumeUnit;
                unitToEdit.PieceUnit = request.pieceUnit;
                
                await db.SaveChangesAsync();
                return Results.Ok();
            });
        }
    }
}