namespace MusicApplicationAPI.Models.DTOs.AlbumDTO
{
    public class AlbumAddDTO
    {
        public string Title { get; set; }

        public int ArtistId { get; set; }

        public string CoverImageUrl { get; set; }
    }
}
