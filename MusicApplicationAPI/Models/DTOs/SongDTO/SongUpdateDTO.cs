using MusicApplicationAPI.Models.Enums;

namespace MusicApplicationAPI.Models.DTOs.SongDTO
{
    public class SongUpdateDTO
    {
        public string Title { get; set; }

        public string Genre { get; set; }

        public string Url { get; set; }

        public int Duration { get; set; }

        public int ArtistId { get; set; }

        public int? AlbumId { get; set; }
    }
}
