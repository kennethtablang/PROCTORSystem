using Microsoft.AspNetCore.Mvc;
using PROCTORSystem.Interfaces;

namespace PROCTORSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditLogsController : ControllerBase
    {
        private readonly IAuditLogService _auditLogService;

        public AuditLogsController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAuditLogs()
        {
            var logs = await _auditLogService.GetAuditLogsAsync();
            return Ok(logs);
        }
    }
}
