using System.ComponentModel.DataAnnotations;

namespace MusicApplicationAPI.Models.DbModels
{
    public class Artist
    {
        [Key]
        public int ArtistId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string Bio { get; set; }

        [Url]
        public string ImageUrl { get; set; }

        public ICollection<Song> Songs { get; set; }
        public ICollection<Album> Albums { get; set; }
    }
}