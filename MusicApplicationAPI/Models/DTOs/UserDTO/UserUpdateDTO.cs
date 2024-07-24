using System.ComponentModel.DataAnnotations;

namespace MusicApplicationAPI.Models.DTOs.UserDTO
{
    public class UserUpdateDTO
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        public DateOnly DOB { get; set; }
    }
}
