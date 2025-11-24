using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Observers
{
    public class SecurityAlertObserver : IAuditObserver
    {
        private readonly ILogger<SecurityAlertObserver> _logger;

        public SecurityAlertObserver(ILogger<SecurityAlertObserver> logger)
        {
            _logger = logger;
        }

        public void OnAuditLogCreated(AuditLog log)
        {
            // Regra: Se for DELETE ou envolver a palavra "admin", dispara alerta
            if (log.Action.ToUpper().Contains("DELETE") || log.EntityName.ToLower().Contains("auth"))
            {
                _logger.LogWarning($"[SECURITY ALERT] Ação crítica detectada! Usuário: {log.UserId}, Ação: {log.Action}");
                // Aqui poderia enviar um e-mail, SMS, Slack, etc.
            }
        }
    }
}