using System.Net;
using System.Text.Json;

namespace LocadoraVeiculos.API.Middleware;

/// <summary>
/// Middleware que captura qualquer exceção não tratada nos controllers
/// e retorna uma resposta JSON padronizada, com o código HTTP apropriado.
/// </summary>
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado: {Message}", ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                sucesso = false,
                mensagem = "Ocorreu um erro interno ao processar a requisição.",
                detalhe = ex.Message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
