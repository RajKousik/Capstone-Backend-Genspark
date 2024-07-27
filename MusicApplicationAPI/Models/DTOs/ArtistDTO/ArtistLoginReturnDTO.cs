namespace MusicApplicationAPI.Models.DTOs.ArtistDTO
{
    public class ArtistLoginReturnDTO
    {
        public int ArtistId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }
    }
}
