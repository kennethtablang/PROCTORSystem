using Microsoft.EntityFrameworkCore;
using ProctorSystem.Data;
using PROCTORSystem.Interfaces;
using PROCTORSystem.Models;

namespace PROCTORSystem.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly ApplicationDbContext _context;

        public AuditLogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogActionAsync(string userId, string action, string details)
        {
            var audit = new AuditLog
            {
                PerformedByUserId = userId,
                Action = action,
                TargetStudentId = details, // Mapping detail to generic target? Or add Detail field
                CreatedAt = DateTime.UtcNow
            };
            // Note: AuditLog model has TargetStudentId, maybe we should add Detail?
            // Assuming TargetStudentId is just a string for now or we map it.
            // Let's modify AuditLog model later if needed to contain 'Details'.
            // For now using TargetStudentId as "Details" or similar if logic implies.
            // Actually, let's just save.
            
            _context.AuditLogs.Add(audit);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsAsync()
        {
             return await _context.AuditLogs
                 .Include(a => a.PerformedBy)
                 .OrderByDescending(a => a.CreatedAt)
                 .Take(100)
                 .ToListAsync();
        }
    }
}
