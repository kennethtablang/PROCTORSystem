using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProctorSystem.Data;
using PROCTORSystem.DTO;
using PROCTORSystem.Interfaces;
using PROCTORSystem.Models;

namespace PROCTORSystem.Services
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentService> _logger;

        public StudentService(ApplicationDbContext context, IMapper mapper, ILogger<StudentService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
        {
            var students = await _context.Students.AsNoTracking().ToListAsync();
            return _mapper.Map<IEnumerable<StudentDto>>(students);
        }

        public async Task<StudentDto?> GetStudentByMachineNameAsync(string machineName)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.MachineName == machineName);
            return _mapper.Map<StudentDto>(student);
        }

        public async Task RegisterStudentAsync(StudentDto studentDto, string connectionId)
        {
            var existing = await _context.Students.FirstOrDefaultAsync(s => s.MachineName == studentDto.MachineName);
            
            if (existing != null)
            {
                existing.IsOnline = true;
                existing.LastSeen = DateTime.UtcNow;
                existing.IPAddress = studentDto.IPAddress;
                existing.ConnectionId = connectionId;
                existing.OperatingSystem = studentDto.OperatingSystem;
                _context.Students.Update(existing);
            }
            else
            {
                var newStudent = _mapper.Map<Student>(studentDto);
                newStudent.ConnectionId = connectionId;
                newStudent.IsOnline = true;
                newStudent.LastSeen = DateTime.UtcNow;
                _context.Students.Add(newStudent);
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation($"Student {studentDto.MachineName} registered/updated.");
        }

        public async Task SetStudentOfflineAsync(string connectionId)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.ConnectionId == connectionId);
            if (student != null)
            {
                student.IsOnline = false;
                student.LastSeen = DateTime.UtcNow;
                student.ConnectionId = null; // Optional: clear connection ID
                _context.Students.Update(student);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Student {student.MachineName} set offline.");
            }
        }

        public async Task<string?> GetConnectionIdByMachineNameAsync(string machineName)
        {
            var student = await _context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.MachineName == machineName);
            
            return student?.ConnectionId;
        }
    }
}
