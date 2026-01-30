using System.ComponentModel.DataAnnotations;

namespace PROCTORSystem.Models
{
    public class Student : BaseEntity
    {
        [Required]
        public string DisplayName { get; set; } = string.Empty;
        [Required]
        public string MachineName { get; set; } = string.Empty;
        [Required]
        public string IPAddress { get; set; } = string.Empty;
        public string? MacAddress { get; set; }
        public string? OperatingSystem { get; set; }
        public bool IsOnline { get; set; } = false;
        public DateTime? LastSeen { get; set; }
        
        public string? ConnectionId { get; set; }
    }
}
