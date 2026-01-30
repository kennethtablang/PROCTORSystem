using PROCTORSystem.DTO;

namespace PROCTORSystem.Interfaces
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(LoginDto loginDto);
    }
}
