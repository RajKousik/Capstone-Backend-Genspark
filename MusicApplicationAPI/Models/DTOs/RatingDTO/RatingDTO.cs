using System.ComponentModel.DataAnnotations;

namespace MusicApplicationAPI.Models.DTOs.RatingDTO
{
    public class RatingDTO
    {
        public int UserId { get; set; }

        public int SongId { get; set; }

        [Range(1, 5)]
        public int RatingValue { get; set; }
    }
}
