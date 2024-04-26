using FrutasColombia_CS_NoSQL_REST_API.DbContexts;
using FrutasColombia_CS_NoSQL_REST_API.Interfaces;
using FrutasColombia_CS_NoSQL_REST_API.Repositories;
using FrutasColombia_CS_NoSQL_REST_API.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//Aqui agregamos los servicios requeridos

//El DBContext a utilizar
builder.Services.AddSingleton<MongoDbContext>();

//Los repositorios
builder.Services.AddScoped<IClimaRepository, ClimaRepository>();
builder.Services.AddScoped<IClasificacionRepository, ClasificacionRepository>();
builder.Services.AddScoped<IEpocaRepository, EpocaRepository>();
builder.Services.AddScoped<IResumenRepository, ResumenRepository>();

//Aqui agregamos los servicios asociados para cada EndPoint
builder.Services.AddScoped<ClimaService>();
builder.Services.AddScoped<ClasificacionService>();
builder.Services.AddScoped<EpocaService>();
builder.Services.AddScoped<ResumenService>();

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Atlas de Frutas Colombianas - MongoDB Version",
        Description = "API para la gestión de frutas colombianas"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Modificamos el encabezado de las peticiones para ocultar el web server utilizado
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Server", "FruitsServer");
    await next();
});



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
