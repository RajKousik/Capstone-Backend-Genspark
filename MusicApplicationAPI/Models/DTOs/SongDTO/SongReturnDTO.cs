using MusicApplicationAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace MusicApplicationAPI.Models.DTOs.SongDTO
{
    public class SongReturnDTO
    {
        [Key]
        public int SongId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public int ArtistId { get; set; }

        public int? AlbumId { get; set; }

        [Required]
        public GenreType Genre { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [Url]
        public string Url { get; set; }
    }
}
