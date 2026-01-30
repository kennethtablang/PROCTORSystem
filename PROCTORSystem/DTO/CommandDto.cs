namespace PROCTORSystem.DTO
{
    public class CommandDto
    {
        public string MachineName { get; set; } = string.Empty;
        public string CommandType { get; set; } = string.Empty; // Lock, Restart, Shutdown
        public string? Payload { get; set; }
    }
}
