using MusicApplicationAPI.Models.DTOs.RatingDTO;

namespace MusicApplicationAPI.Interfaces.Service
{
    public interface IRatingService
    {
        Task<RatingReturnDTO> AddRating(RatingDTO ratingDTO);
        Task<RatingReturnDTO> UpdateRating(RatingDTO ratingDTO);
        Task<IEnumerable<RatingReturnDTO>> GetRatingsBySongId(int songId);
        Task<IEnumerable<RatingReturnDTO>> GetRatingsByUserId(int userId);
        Task DeleteRating(int userId, int songId);
        Task<IEnumerable<SongRatingDTO>> TopRatedSongs();
    }
}
