using ApiCatalog.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalog.ApiEndpoints
{
    public static class ProductsEndpoints
    {
        public static void MapProductsEndpoints(this WebApplication app)
        {
            app.MapGet("/products", async (AppDbContext db) => await db.Products.ToListAsync());

            app.MapGet("/products/{id:int}", async (int id, AppDbContext db) => {
                return await db.Products.FindAsync(id)
                             is Product Products
                             ? Results.Ok(Products)
                             : Results.NotFound();
            });

            app.MapPost("/products", async (Product product, AppDbContext db) => {
                db.Products.Add(product);
                await db.SaveChangesAsync();
                return Results.Created($"/products/{product.ProductId}", product);
            });

            app.MapPut("/products/{id:int}", async (int id, Product product, AppDbContext db) =>
            {
                if (product.CategoryId != id)
                {
                    return Results.BadRequest();
                }

                var productDb = await db.Products.FindAsync(id);

                if (productDb is null) return Results.NotFound();

                productDb.Name = product.Name;
                productDb.Description = product.Description;
                productDb.Price = product.Price;
                productDb.Image = product.Image;
                productDb.BoughtDate = product.BoughtDate;
                productDb.Stock = product.Stock;
                productDb.CategoryId = product.CategoryId;

                await db.SaveChangesAsync();
                return Results.Ok(productDb);
            });

            app.MapDelete("/products/{id:int}", async (int id, AppDbContext db) =>
            {
                var product = await db.Products.FindAsync(id);

                if (product is null)
                {
                    return Results.NotFound();
                }

                db.Products.Remove(product);
                await db.SaveChangesAsync();

                return Results.NoContent();
            });
        }
    }
}
