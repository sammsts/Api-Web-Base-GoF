using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ApiWebBase.Factories
{
    public interface IAuditLogFactory
    {
        Task<AuditLog> CreateFromContextAsync(HttpContext context);
    }

    public class AuditLogFactory : IAuditLogFactory
    {
        public async Task<AuditLog> CreateFromContextAsync(HttpContext context)
        {
            var request = context.Request;
            var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "anonymous";
            var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var userAgent = request.Headers["User-Agent"].ToString();
            var entityName = request.Path.Value?.Split('/').Skip(1).FirstOrDefault() ?? "unknown";
            var entityId = context.Request.RouteValues["id"]?.ToString() ?? "";

            string bodyAsText = string.Empty;
            if (request.Body.CanSeek)
            {
                request.Body.Position = 0;
                using var reader = new StreamReader(request.Body, leaveOpen: true);
                bodyAsText = await reader.ReadToEndAsync();
                request.Body.Position = 0;
            }

            return new AuditLog
            {
                UserId = userId,
                Action = $"{request.Method} {request.Path}",
                EntityName = entityName,
                EntityId = entityId,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                NewValues = bodyAsText
            };
        }
    }
}