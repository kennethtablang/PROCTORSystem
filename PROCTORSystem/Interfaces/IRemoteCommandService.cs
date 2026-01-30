using PROCTORSystem.DTO;

namespace PROCTORSystem.Interfaces
{
    public interface IRemoteCommandService
    {
        Task<CommandDto> CreateCommandAsync(CommandDto commandDto);
        Task UpdateCommandStatusAsync(Guid commandId, string status, string? result);
    }
}
