using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NeuroSync.Application.Common;

namespace NeuroSync.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, "Erro de negócio.");
                await WriteProblemDetails(context, ex.StatusCode, "Erro de negócio", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado.");
                await WriteProblemDetails(context, HttpStatusCode.InternalServerError, "Erro inesperado", "Ocorreu um erro ao processar sua solicitação.");
            }
        }

        private static async Task WriteProblemDetails(HttpContext context, HttpStatusCode status, string title, string detail)
        {
            context.Response.StatusCode = (int)status;
            context.Response.ContentType = "application/problem+json";

            var problem = new
            {
                type = "https://httpstatuses.io/" + (int)status,
                title,
                status = (int)status,
                detail,
                instance = context.Request.Path
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
    }
}
