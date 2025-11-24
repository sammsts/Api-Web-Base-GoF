using Domain.Entities;

namespace Application.Observers
{
    public interface IAuditObserver
    {
        void OnAuditLogCreated(AuditLog log);
    }
}