namespace MusicApplicationAPI.Models.DTOs.AlbumDTO
{
    public class AlbumReturnDTO
    {
        public int AlbumId { get; set; }
        public string Title { get; set; }

        public int ArtistId { get; set; }

        public string CoverImageUrl { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}
