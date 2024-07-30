using MusicApplicationAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

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
        public string Genre { get; set; }

        public int Duration { get; set; }

        [Required]
        [Url]
        public string Url { get; set; }

        public string ImageUrl { get; set; }
    }
}
