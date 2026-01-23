using DocumentFormat.OpenXml.Office2021.PowerPoint.Comment;
using PROCTORSystem.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROCTORSystem.Models
{
    public class RemoteCommand : BaseEntity
    {
        [Required]
        public Guid StudentId { get; set; }


        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; }


        public CommandType CommandType { get; set; }


        // Optional JSON or string payload (e.g. app name)
        public string? Payload { get; set; }


        public CommandStatus Status { get; set; } = CommandStatus.Pending;


        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExecutedAt { get; set; }
    }
}
