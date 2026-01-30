using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using PROCTORSystem.DTO;
using PROCTORSystem.Interfaces;

namespace PROCTORSystem.Hubs
{
    public class MonitoringHub : Hub
    {
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;

        public MonitoringHub(IStudentService studentService, AutoMapper.IMapper mapper)
        {
            _studentService = studentService;
            _mapper = mapper;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await _studentService.SetStudentOfflineAsync(Context.ConnectionId);
            // We might need to know WHICH student went offline to notify clients efficiently
            // For now, clients can refresh or we can broadcast a generic "Refresh" or find by ID if we returned it.
            // Simplified: Broadcast to Admins to refresh list
            await Clients.Group("Admins").SendAsync("ForceRefresh"); 

            await base.OnDisconnectedAsync(exception);
        }

        public async Task RegisterStudent(StudentDto studentDto)
        {
            await _studentService.RegisterStudentAsync(studentDto, Context.ConnectionId);
            await Clients.Group("Admins").SendAsync("StudentOnline", studentDto);
        }

        public async Task StreamScreen(string machineName, string base64Image)
        {
            await Clients.Group("Admins").SendAsync("ReceiveScreen", machineName, base64Image);
        }

        public async Task JoinAdminGroup()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
            var students = await _studentService.GetAllStudentsAsync();
            await Clients.Caller.SendAsync("ActiveStudents", students);
        }

        public async Task SendCommandToStudent(string machineName, string command, string payload)
        {
             var connectionId = await _studentService.GetConnectionIdByMachineNameAsync(machineName);
             if (!string.IsNullOrEmpty(connectionId))
             {
                 await Clients.Client(connectionId).SendAsync("ReceiveCommand", command, payload);
             }
        }
    }
}
