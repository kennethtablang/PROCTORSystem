using PROCTORSystem.DTO;
using PROCTORSystem.Models;

namespace PROCTORSystem.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDto>> GetAllStudentsAsync();
        Task<StudentDto?> GetStudentByMachineNameAsync(string machineName);
        Task RegisterStudentAsync(StudentDto studentDto, string connectionId);
        Task SetStudentOfflineAsync(string connectionId);
        Task<string?> GetConnectionIdByMachineNameAsync(string machineName);
    }
}
