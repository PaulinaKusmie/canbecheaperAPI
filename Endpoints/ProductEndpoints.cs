using System.Text.RegularExpressions;
using canbecheaperAPI.DTO.Product;
using canbecheaperAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace canbecheaperAPI.Endpoints
{
    public static class ProductEndpoints
    {
        public static void MapProductEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/products")
                .WithTags("Products")
                .WithOpenApi();


            group.MapPost("/save", async (ProductRequest request, DbOliwia230Context db) =>
             {

                 var productToCheck = await db.CheaperProducts.FirstOrDefaultAsync(p => p.Name == request.name.Trim().ToLower());
                 if(productToCheck is not null) 
                     return Results.Conflict(new { message = "Produkt o tej nazwie już istnieje" });


                 CheaperProduct newProduct = new CheaperProduct
                 {
                     Name = request.name.Trim().ToLower(),
                 };

                 db.CheaperProducts.Add(newProduct);
                 await db.SaveChangesAsync();
                 return Results.Created($"/api/products/{newProduct.Id}", new { id = newProduct.Id, name = newProduct.Name});
             });




            group.MapGet("/{searchText}", async (string searchText, DbOliwia230Context db) =>
            {
                if (string.IsNullOrWhiteSpace(searchText))
                    return Results.BadRequest(new { message = "Nieprawidłowe dane" });

                List<CheaperProduct> cheaperProducts = await db.CheaperProducts
                .Where(p => p.Name.Contains(searchText.ToLower()))
                .ToListAsync();

                return Results.Ok(cheaperProducts.Select(p => new ProductDTO(p.Id, p.Name)));
            });
        }
    }
}
