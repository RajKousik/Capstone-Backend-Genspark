using MusicApplicationAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace MusicApplicationAPI.Models.DTOs.UserDTO
{
    public class UserRegisterReturnDTO
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public RoleType Role { get; set; } // Admin, Normal User, Premium User

        [Required]
        public DateTime DOB { get; set; }
    }
}
