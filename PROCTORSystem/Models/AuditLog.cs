using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROCTORSystem.Models
{
    public class AuditLog : BaseEntity
    {
        [Required]
        public string PerformedByUserId { get; set; }

        [ForeignKey(nameof(PerformedByUserId))]
        public ApplicationUser PerformedBy { get; set; }

        [Required]
        public string Action { get; set; } = string.Empty;

        // Optional: link to a target student (Id as string) or other context
        public string? TargetStudentId { get; set; }
    }
}
