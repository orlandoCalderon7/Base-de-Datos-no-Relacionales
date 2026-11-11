namespace MascotasService.Middlewares;

using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

public class ManejadorExcepcionesMiddleware(RequestDelegate next, ILogger<ManejadorExcepcionesMiddleware> logger, IHostEnvironment env)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Excepción no controlada en la ruta {Path}: {Message}", context.Request.Path, ex.Message);
            await ManejarExcepcionAsync(context, ex, env);
        }
    }

    private static Task ManejarExcepcionAsync(HttpContext context, Exception excepcion, IHostEnvironment env)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var problema = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Error Interno en el Servidor",
            Detail = env.IsDevelopment() ? excepcion.Message : "Ha ocurrido un error inesperado. Por favor, intente nuevamente o contacte al soporte.",
            Instance = context.Request.Path
        };

        if (excepcion is InvalidOperationException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            problema.Status = StatusCodes.Status400BadRequest;
            problema.Title = "Operación de Negocio Inválida";
            problema.Detail = excepcion.Message;
        }

        var opciones = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        return context.Response.WriteAsync(JsonSerializer.Serialize(problema, opciones));
    }
}
