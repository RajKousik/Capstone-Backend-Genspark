using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MusicApplicationAPI.Models.DbModels
{
    public class Favorite
    {
        [Key]
        public int FavoriteId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public int? SongId { get; set; }

        [ForeignKey(nameof(SongId))]
        public Song? Song { get; set; }

        public int? PlaylistId { get; set; }

        [ForeignKey(nameof(PlaylistId))]
        public Playlist? Playlist { get; set; }
    }
}
