using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PROCTORSystem.DTO;
using PROCTORSystem.Interfaces;

namespace PROCTORSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly IRemoteCommandService _commandService;
        private readonly IStudentService _studentService;
        private readonly Microsoft.AspNetCore.SignalR.IHubContext<PROCTORSystem.Hubs.MonitoringHub> _hubContext;

        public CommandsController(
            IRemoteCommandService commandService, 
            IStudentService studentService,
            Microsoft.AspNetCore.SignalR.IHubContext<PROCTORSystem.Hubs.MonitoringHub> hubContext)
        {
            _commandService = commandService;
            _studentService = studentService;
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<ActionResult<CommandDto>> SendCommand(CommandDto commandDto)
        {
            try
            {
                var result = await _commandService.CreateCommandAsync(commandDto);
                
                // Dispatch via SignalR
                var connectionId = await _studentService.GetConnectionIdByMachineNameAsync(commandDto.MachineName);
                if (!string.IsNullOrEmpty(connectionId))
                {
                    await _hubContext.Clients.Client(connectionId)
                        .SendAsync("ReceiveCommand", commandDto.CommandType, commandDto.Payload);
                    
                    await _commandService.UpdateCommandStatusAsync(Guid.NewGuid(), "Sent", null); // Example update
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
