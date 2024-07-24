namespace MusicApplicationAPI.Models.DTOs.RatingDTO
{
    public class SongRatingDTO
    {
        public int SongId { get; set; }
        public string Title { get; set; } // Example field for the song title
        public double AverageRating { get; set; }
    }
}
