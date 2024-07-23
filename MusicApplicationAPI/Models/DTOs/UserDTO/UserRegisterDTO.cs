using MusicApplicationAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MusicApplicationAPI.Models.DTOs.UserDTO
{
    public class UserRegisterDTO
    {

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public DateOnly DOB { get; set; }
    }
}
