using Microsoft.EntityFrameworkCore;
using ProctorSystem.Data;
using PROCTORSystem.DTO;
using PROCTORSystem.Enum;
using PROCTORSystem.Interfaces;
using PROCTORSystem.Models;

namespace PROCTORSystem.Services
{
    public class RemoteCommandService : IRemoteCommandService
    {
        private readonly ApplicationDbContext _context;

        public RemoteCommandService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CommandDto> CreateCommandAsync(CommandDto commandDto)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.MachineName == commandDto.MachineName);
            if (student == null)
            {
                throw new Exception("Student not found");
            }

            if (!System.Enum.TryParse(commandDto.CommandType, true, out CommandType type))
            {
                type = CommandType.Custom; // Fallback or throw
            }

            var command = new RemoteCommand
            {
                StudentId = student.Id,
                CommandType = type,
                Payload = commandDto.Payload,
                Status = CommandStatus.Pending,
                IssuedAt = DateTime.UtcNow
            };

            _context.RemoteCommands.Add(command);
            await _context.SaveChangesAsync();

            return commandDto;
        }

        public async Task UpdateCommandStatusAsync(Guid commandId, string status, string? result)
        {
            var command = await _context.RemoteCommands.FindAsync(commandId);
            if (command != null && System.Enum.TryParse(status, true, out CommandStatus statusEnum))
            {
                command.Status = statusEnum;
                command.ExecutedAt = DateTime.UtcNow; // Or separate execution time
                // command.Result = result; // If we add a result field
                await _context.SaveChangesAsync();
            }
        }
    }
}
