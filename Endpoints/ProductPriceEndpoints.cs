using System.Transactions;
using canbecheaperAPI.DTO.ProductPrice;
using canbecheaperAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace canbecheaperAPI.Endpoints
{
    public static class ProductPriceEndpoints
    {
        public static void MapProductPriceEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/productprices")
                 .WithTags("Prices")
                 .WithOpenApi();

            group.MapGet("/{userId}/{typeId}", async (int userId, int typeId, DbOliwia230Context db) =>
            {
                try
                {
                                        var productPriceResponses = await (
                    from pp in db.CheaperProductPrices
                    where pp.UserId == userId && pp.TypeId == typeId
                    join product in db.CheaperProducts on pp.ProductId equals product.Id
                    join price in db.CheaperPrices on pp.PriceId equals price.Id
                    select new ProductPriceResponse(
                    pp.Id,
                    pp.ProductId,
                    pp.PriceId,
                    pp.TypeId,
                    pp.UserId,
                    pp.CreatedAt,
                    product.Name,
                    price.Price)).ToListAsync();

                    return Results.Ok(productPriceResponses);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Błąd podczas tworzenia: {ex.Message}");
                }
            });

            group.MapPost("/save", async (ProductPriceRequest request, DbOliwia230Context db) =>
            {

 
                if (request.productId == null && string.IsNullOrWhiteSpace(request.name))
                    return Results.BadRequest(new { message = "Podaj productId lub nazwę produktu." });

                if (request.price <= 0)
                    return Results.BadRequest(new { message = "Cena musi być większa od 0." });

                
                using var trans = await db.Database.BeginTransactionAsync();
                try { 

                    CheaperPrice cheaperPrice = new CheaperPrice { Price = request.price };
                    db.CheaperPrices.Add(cheaperPrice);

                    CheaperProduct product;
                    if (request.productId != null)
                    {
                        product = await db.CheaperProducts.FindAsync(request.productId);
                    }
                    else
                    {
                        product = new CheaperProduct
                        {
                            Name = request.name.Trim().ToLower(),
                        };
                        db.CheaperProducts.Add(product);
                    }

                    await db.SaveChangesAsync();

                    CheaperProductPrice newProductPrice = new CheaperProductPrice
                    {
                        ProductId = request.productId ?? product.Id,
                        PriceId = cheaperPrice.Id,
                        TypeId = request.typeId,
                        UserId = request.userId,
                        CreatedAt = DateTime.UtcNow
                    };
                    db.CheaperProductPrices.Add(newProductPrice);


                    await db.SaveChangesAsync();
                    await trans.CommitAsync();


                    return Results.Created();

                } catch(Exception ex) {

                    return Results.Problem($"Błąd podczas tworzenia: {ex.Message}");
                }

            }).WithSummary("Create product price"); ;

            group.MapDelete("delete/{id}", async (int id, DbOliwia230Context db) =>
            {

                try
                {
                    CheaperProductPrice cheaperProductPrice = await db.CheaperProductPrices.FindAsync(id);
                    if (cheaperProductPrice == null) return Results.NotFound(new { message = "Brak rekordu" });

                    CheaperPrice cheaperPrice = await db.CheaperPrices.FindAsync(cheaperProductPrice.ProductId);
                    if (cheaperPrice == null) return Results.NotFound(new { message = "Brak ceny" });

                    db.CheaperProductPrices.Remove(cheaperProductPrice);
                    db.CheaperPrices.Remove(cheaperPrice);

                    await db.SaveChangesAsync();
                    return Results.NoContent();
                }
                catch (Exception ex) { 

                    return Results.Problem($"Błąd podczas tworzenia: {ex.Message}");
                }
                
            });

        }
    }
}
