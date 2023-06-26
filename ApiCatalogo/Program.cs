using ApiCatalog.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

//Endpoints
app.MapGet("/categories", async(AppDbContext db) => await db.Categories.ToListAsync());

app.MapGet("/categories/{id:int}", async (int id, AppDbContext db) => {
    return await db.Categories.FindAsync(id)
                 is Category category
                 ? Results.Ok(category)
                 : Results.NotFound();
});

app.MapPost("/categories", async(Category category, AppDbContext db) => {
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

    if(category is null)
    {
        return Results.NotFound();
    }

    db.Categories.Remove(category);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();