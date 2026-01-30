using Microsoft.AspNetCore.Mvc;
using PROCTORSystem.DTO;
using PROCTORSystem.Interfaces;

namespace PROCTORSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(students);
        }

        [HttpGet("{machineName}")]
        public async Task<ActionResult<StudentDto>> GetStudent(string machineName)
        {
            var student = await _studentService.GetStudentByMachineNameAsync(machineName);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }
    }
}
