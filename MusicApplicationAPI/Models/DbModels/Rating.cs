using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MusicApplicationAPI.Models.DbModels
{
    public class Rating
    {
        [Key]
        public int RatingId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [Required]
        public int SongId { get; set; }

        [ForeignKey(nameof(SongId))]
        public Song Song { get; set; }

        [Required]
        [Range(1, 5)]
        public int RatingValue { get; set; }
    }
}
