using ApiWebBase.Factories;
using Application.Interfaces;

namespace ApiWebBase.Middleware
{
    public class AuditMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuditMiddleware> _logger;

        public AuditMiddleware(RequestDelegate next, ILogger<AuditMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IAuditService auditService, IAuditLogFactory auditFactory)
        {
            context.Request.EnableBuffering();

            await _next(context);

            // Regra simplificada
            if (context.Request.Method != HttpMethods.Get && context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)
            {
                try
                {
                    // FACTORY METHOD: Criação delegada
                    var auditLog = await auditFactory.CreateFromContextAsync(context);
                    await auditService.LogAsync(auditLog);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao auditar");
                }
            }
        }
    }
}