using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MusicApplicationAPI.Models.DbModels
{
    public class Album
    {
        [Key]
        public int AlbumId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public int ArtistId { get; set; }

        [ForeignKey(nameof(ArtistId))]
        public Artist Artist { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Url]
        public string CoverImageUrl { get; set; }

        public ICollection<Song> Songs { get; set; }
    }
}