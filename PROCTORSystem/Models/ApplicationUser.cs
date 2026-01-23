using Microsoft.AspNetCore.Identity;
using PROCTORSystem.Enum;
using System.ComponentModel.DataAnnotations;

namespace PROCTORSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
