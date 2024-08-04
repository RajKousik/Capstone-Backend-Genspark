using MusicApplicationAPI.Models.Enums;

namespace MusicApplicationAPI.Models.DTOs.ArtistDTO
{
    public class ArtistLoginReturnDTO
    {
        public int ArtistId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }
        public RoleType Role { get; set; }

        public string ImageUrl { get; set; }    
    }
}
