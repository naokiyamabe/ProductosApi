using System.Reflection;
using Microsoft.OpenApi.Models;
using ProductosApi.Data;
using ProductosApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Conexión a MySQL
builder.Services.AddSingleton<ConexionDb>();

// Repositorio: una sola instancia que cumple ambas interfaces
builder.Services.AddScoped<ProductoRepository>();
builder.Services.AddScoped<IProductoRepository>(sp => sp.GetRequiredService<ProductoRepository>());
builder.Services.AddScoped<IProductoBusqueda>(sp => sp.GetRequiredService<ProductoRepository>());

builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opciones =>
{
    opciones.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API de Productos",
        Version = "v1",
        Description = "Backend CRUD de productos con ASP.NET Core 8 y MySQL."
    });

    var archivoXml = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
    if (File.Exists(archivoXml))
    {
        opciones.IncludeXmlComments(archivoXml);
    }
});

var app = builder.Build();

// Swagger habilitado siempre para facilitar las pruebas de la actividad
app.UseSwagger();
app.UseSwaggerUI(opciones =>
{
    opciones.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Productos v1");
});

app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.MapControllers();

app.Run();
