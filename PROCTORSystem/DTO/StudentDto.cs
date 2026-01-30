namespace PROCTORSystem.DTO
{
    public class StudentDto
    {
        public Guid Id { get; set; }
        public string MachineName { get; set; } = string.Empty;
        public string IPAddress { get; set; } = string.Empty;
        public string? OperatingSystem { get; set; }
        public bool IsOnline { get; set; }
        public DateTime? LastSeen { get; set; }
    }
}
