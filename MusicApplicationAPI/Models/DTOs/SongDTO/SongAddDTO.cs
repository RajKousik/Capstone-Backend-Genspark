using MusicApplicationAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace MusicApplicationAPI.Models.DTOs.SongDTO
{
    public class SongAddDTO
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public int ArtistId { get; set; }

        public int? AlbumId { get; set; }

        [Required]
        public GenreType Genre { get; set; }

        public TimeSpan Duration { get; set; }

        [Required]
        [Url]
        public string Url { get; set; }
    }
}
