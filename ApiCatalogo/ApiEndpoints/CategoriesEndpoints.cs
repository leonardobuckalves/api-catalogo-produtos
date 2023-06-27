using ApiCatalog.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalog.ApiEndpoints
{
    public static class CategoriesEndpoints
    {
        public static void MapCategoriesEndPoints(this WebApplication app)
        {
            app.MapGet("/categories", async (AppDbContext db) => await db.Categories.ToListAsync());

            app.MapGet("/categories/{id:int}", async (int id, AppDbContext db) => {
                return await db.Categories.FindAsync(id)
                             is Category category
                             ? Results.Ok(category)
                             : Results.NotFound();
            });

            app.MapPost("/categories", async (Category category, AppDbContext db) => {
                db.Categories.Add(category);
                await db.SaveChangesAsync();
                return Results.Created($"/categories/{category.CategoryId}", category);
            });

            app.MapPut("/categories/{id:int}", async (int id, Category category, AppDbContext db) =>
            {
                if (category.CategoryId != id)
                {
                    return Results.BadRequest();
                }

                var categoryDB = await db.Categories.FindAsync(id);

                if (categoryDB is null) return Results.NotFound();

                categoryDB.Name = category.Name;
                categoryDB.Description = category.Description;

                await db.SaveChangesAsync();
                return Results.Ok(categoryDB);
            });

            app.MapDelete("/categories/{id:int}", async (int id, AppDbContext db) =>
            {
                var category = await db.Categories.FindAsync(id);

                if (category is null)
                {
                    return Results.NotFound();
                }

                db.Categories.Remove(category);
                await db.SaveChangesAsync();

                return Results.NoContent();
            });
        }
    }
}
