using Domain.Entities;
namespace Application.Interfaces;

public interface IAuditService
{
    Task LogAsync(AuditLog log);
}

