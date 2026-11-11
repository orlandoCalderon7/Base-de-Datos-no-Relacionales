using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using MascotasService.Application.Interfaces;
using MascotasService.Application.Services;
using MascotasService.Infrastructure.Data;
using MascotasService.Infrastructure.Repositories;
using MascotasService.Infrastructure.Services;
using MascotasService.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// 1. Base de datos PostgreSQL con EF Core (Capa Infrastructure)
var connString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Host=localhost;Port=5432;Database=adoptamas_db;Username=postgres;Password=ZegelSecretPassword2026!";

builder.Services.AddDbContext<AdoptaMasDbContext>(options =>
    options.UseNpgsql(connString));

// 2. Inyección de Dependencias
builder.Services.AddScoped<IMascotaRepositorio, MascotaRepositorio>();
builder.Services.AddScoped<IMascotaServicio, MascotaServicio>();
builder.Services.AddScoped<ISolicitudServicio, SolicitudServicio>();

// 3. Seguridad JWT
var jwtKey = builder.Configuration["Jwt:SecretKey"] ?? "ClaveSecretaSuperSeguraZegelDesarrolloServiciosWeb2026!";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "ZegelAdoptaMasAPI",
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "ZegelEstudiantes",
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// 4. Controladores, CORS y OpenAPI
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// 5. Middleware Pipeline
app.UseMiddleware<ManejadorExcepcionesMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("AdoptaMas API - Mascotas & Solicitudes")
               .WithTheme(ScalarTheme.Purple)
               .WithPreferredScheme("Bearer");
    });
}

app.UseCors("PermitirTodo");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Inicializar base de datos
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AdoptaMasDbContext>();
    try
    {
        db.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogWarning(ex, "No se pudo conectar a PostgreSQL al iniciar. Verifique si el contenedor Docker está activo.");
    }
}

app.Run();
