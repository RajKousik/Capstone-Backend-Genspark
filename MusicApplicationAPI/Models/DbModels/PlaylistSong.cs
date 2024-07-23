using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MusicApplicationAPI.Models.DbModels
{
    public class PlaylistSong
    {
        [Key]
        public int PlaylistSongId { get; set; }

        [Required]
        public int PlaylistId { get; set; }

        [ForeignKey(nameof(PlaylistId))]
        public Playlist Playlist { get; set; }

        [Required]
        public int SongId { get; set; }

        [ForeignKey(nameof(SongId))]
        public Song Song { get; set; }
    }
}
