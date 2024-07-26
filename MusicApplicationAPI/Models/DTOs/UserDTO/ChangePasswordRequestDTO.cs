using System.ComponentModel.DataAnnotations;

namespace MusicApplicationAPI.Models.DTOs.UserDTO
{
    public class ChangePasswordRequestDTO
    {
        [Required] 
        public int UserId { get; set; }
        [Required] 
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
