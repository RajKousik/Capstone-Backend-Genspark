using MusicApplicationAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace MusicApplicationAPI.Models.DTOs.ArtistDTO
{
    public class ArtistReturnDTO
    {
        public int ArtistId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string ImageUrl { get; set; }

        public string Status { get; set; }

        [Required]
        public RoleType Role { get; set; }
    }
}
