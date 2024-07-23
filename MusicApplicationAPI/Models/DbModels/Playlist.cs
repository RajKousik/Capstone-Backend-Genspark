using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MusicApplicationAPI.Models.DbModels
{
    public class Playlist
    {
        [Key]
        public int PlaylistId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        public ICollection<PlaylistSong> PlaylistSongs { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
    }
}
