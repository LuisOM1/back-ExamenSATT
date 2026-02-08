using Microsoft.AspNetCore.Mvc;

namespace ExamenSATT.Middleware
{
    // Excepcion global para los controllers que se reutiliza en los try catch (Pro)
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try {
                await _next(context);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Excepción no controlada en la ruta: {Path}", context.Request.Path);
                context.Response.ContentType = "application/problem+json"; // Estándar Pro

                // 1 senior devuelve 1 problemdetails no 1 json cualquiera de error
                var problem = new ProblemDetails {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Error Interno",
                    Detail = ex.Message,  // En producción, no envíes el mensaje detallado por seguridad
                    Instance = context.Request.Path
                };

                await context.Response.WriteAsJsonAsync(problem);
            }
        }
    }
}
