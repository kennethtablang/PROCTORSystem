using PROCTORSystem.DTO;
using PROCTORSystem.Models;

namespace PROCTORSystem.Interfaces
{
    public interface IAuditLogService
    {
        Task LogActionAsync(string userId, string action, string details);
        Task<IEnumerable<AuditLog>> GetAuditLogsAsync();
    }
}
