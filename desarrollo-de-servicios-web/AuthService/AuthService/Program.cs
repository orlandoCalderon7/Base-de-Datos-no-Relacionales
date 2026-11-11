using Scalar.AspNetCore;
using AuthService.Application.Interfaces;
using AuthService.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Inyección de Dependencias (Capa Application e Infrastructure)
builder.Services.AddSingleton<ITokenServicio, TokenServicio>();

// 2. Controladores, CORS y OpenAPI
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

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("AdoptaMas API - Autenticación & JWT")
               .WithTheme(ScalarTheme.Purple);
    });
}

app.UseCors("PermitirTodo");
app.MapControllers();

app.Run();
