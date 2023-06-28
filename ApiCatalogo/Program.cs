using ApiCatalog.ApiEndpoints;
using ApiCatalog.AppServicesExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiSwagger();
builder.AddPersistence();
builder.Services.AddCors();

var app = builder.Build();

app.MapCategoriesEndPoints();
app.MapProductsEndpoints();

var environment = app.Environment;

app.UseExceptionHandling(environment)
    .UseSwaggerMiddleware()
    .UseAppCors();

app.Run();