using System.ComponentModel.DataAnnotations;

namespace MusicApplicationAPI.Models.DTOs.RatingDTO
{
    public class RatingReturnDTO
    {
        public int RatingId { get; set; }
        public int UserId { get; set; }

        public int SongId { get; set; }

        [Range(1, 5)]
        public int RatingValue { get; set; }
    }
}
