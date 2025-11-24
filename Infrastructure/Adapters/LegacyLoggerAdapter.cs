using Application.Observers;
using Domain.Entities;
using Infrastructure.LegacySystem;

// O Adapter(Faz o Legado parecer um Observer nosso)
namespace Infrastructure.Adapters
{
    public class LegacyLoggerAdapter : IAuditObserver
    {
        private readonly LegacyXmlLogger _legacyLogger;

        public LegacyLoggerAdapter()
        {
            _legacyLogger = new LegacyXmlLogger(); // Ou injetado se fosse registrado
        }

        public void OnAuditLogCreated(AuditLog log)
        {
            // ADAPTER: Converte nosso Objeto AuditLog para o formato XML que o legado espera
            string xmlData = $"<log><user>{log.UserId}</user><action>{log.Action}</action></log>";

            // Delega a chamada
            _legacyLogger.WriteXmlLog(xmlData);
        }
    }
}