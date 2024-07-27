using System.ComponentModel.DataAnnotations;

namespace MusicApplicationAPI.Models.DTOs.ArtistDTO
{
    public class ArtistAddDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string Email {  get; set; }
        public string Password { get; set; }
        public string Bio { get; set; }
        public string ImageUrl { get; set; }
    }
}
