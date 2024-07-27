using System.ComponentModel.DataAnnotations;

namespace MusicApplicationAPI.Models.DTOs.OtherDTO
{
    public class ChangePasswordRequestDTO
    {
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
