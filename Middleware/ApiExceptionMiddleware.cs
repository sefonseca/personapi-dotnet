using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Exceptions;

namespace personapi_dotnet.Middleware;

/// <summary>
/// Manejo global de errores para la API REST (rutas /api/...).
/// Convierte las excepciones en respuestas JSON con formato ProblemDetails (RFC 9457).
/// Las rutas MVC no se tocan: sus errores los maneja UseExceptionHandler.
/// </summary>
public class ApiExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiExceptionMiddleware> _logger;

    public ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex) when (context.Request.Path.StartsWithSegments("/api") && !context.Response.HasStarted)
        {
            var (status, title) = ex switch
            {
                AppException app => (app.StatusCode, app.Message),
                DbUpdateException => (StatusCodes.Status409Conflict,
                    "No se pudo guardar el cambio por una restricción de la base de datos."),
                _ => (StatusCodes.Status500InternalServerError, "Ocurrió un error interno en el servidor.")
            };

            if (status >= 500)
                _logger.LogError(ex, "Error no controlado en {Path}", context.Request.Path);
            else
                _logger.LogWarning("Solicitud rechazada en {Path}: {Message}", context.Request.Path, ex.Message);

            var problem = new ProblemDetails
            {
                Status = status,
                Title = title,
                Instance = context.Request.Path
            };

            context.Response.StatusCode = status;
            await context.Response.WriteAsJsonAsync(problem, options: null, contentType: "application/problem+json");
        }
    }
}
