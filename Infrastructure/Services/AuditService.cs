using Application.Interfaces;
using Application.Observers;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Services
{
    public class AuditService : IAuditService
    {
        private readonly ApplicationDbContext _context;
        // Lista de observadores
        private readonly IEnumerable<IAuditObserver> _observers;
        public AuditService(ApplicationDbContext context, IEnumerable<IAuditObserver> observers)
        {
            _context = context;
            _observers = observers;
        }

        public async Task LogAsync(AuditLog log)
        {
            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();

            // OBSERVER: Notifica todos os interessados
            foreach (var observer in _observers)
            {
                try
                {
                    observer.OnAuditLogCreated(log);
                }
                catch { /* Não queremos que uma falha no alerta pare o fluxo */ }
            }
        }
    }
}
