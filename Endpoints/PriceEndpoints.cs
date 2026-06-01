using canbecheaperAPI.DTO.Price;
using canbecheaperAPI.Models;

namespace canbecheaperAPI.Endpoints
{
    public static class PriceEndpoints
    {
        public static void MapPriceEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/prices")
                .WithTags("Prices")
                .WithOpenApi();

            group.MapPatch("/update", async (PriceRequest request, DbOliwia230Context db) =>
            {
                CheaperPrice? price = await db.CheaperPrices.FindAsync(request.id);
                if (price is null)
                    return Results.NotFound(new { message = $"Cena nie istnieje {request.id} " });


                price.Price = request.price;
                db.CheaperPrices.Update(price);
                await db.SaveChangesAsync();

                return Results.NoContent();

            })
            .WithSummary("Update price");


            group.MapPut("/save", async (PriceRequest request, DbOliwia230Context db) =>
            {
                CheaperPrice cheaperPrice = new CheaperPrice
                {
                    Price = request.price
                };

                db.CheaperPrices.Add(cheaperPrice);
                await db.SaveChangesAsync();
                return Results.Ok(cheaperPrice);

            }).WithSummary("Create price");


        }
    }
}
