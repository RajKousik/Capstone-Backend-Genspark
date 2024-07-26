using MusicApplicationAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace MusicApplicationAPI.Models.DTOs.UserDTO
{
    public class UserLoginReturnDTO
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public RoleType Role { get; set; } // Admin, Normal User, Premium User
        [Required]
        public string Token { get; set; }
        public bool IsPremiumExpired { get; set; } // New flag for premium status
        public string Message { get; set; } // Optional message for users
    }
}
