using MusicApplicationAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace MusicApplicationAPI.Models.DTOs.UserDTO
{
    public class UserLoginDTO
    {

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }


        public string Password{ get; set; }

    }
}
