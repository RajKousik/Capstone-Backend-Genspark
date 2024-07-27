using Microsoft.AspNetCore.Mvc;
using MusicApplicationAPI.Exceptions.RatingExceptions;
using MusicApplicationAPI.Exceptions.SongExceptions;
using MusicApplicationAPI.Exceptions.UserExceptions;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Models.DTOs.RatingDTO;
using WatchDog;
using MusicApplicationAPI.Models.ErrorModels;
using Microsoft.AspNetCore.Authorization;

namespace MusicApplicationAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v1/ratings")]
    public class RatingsController : ControllerBase
    {
        #region Private Fields

        private readonly IRatingService _ratingService;
        private readonly ILogger<RatingsController> _logger;

        #endregion

        #region Constructor

        public RatingsController(IRatingService ratingService, ILogger<RatingsController> logger)
        {
            _ratingService = ratingService;
            _logger = logger;
        }

        #endregion

        #region Public Endpoints

        /// <summary>
        /// Adds a rating for a song.
        /// </summary>
        /// <param name="ratingDTO">The rating details.</param>
        /// <returns>The added rating details.</returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(RatingDTO), 200)]
        [ProducesResponseType(typeof(ErrorModel), 404)]
        [ProducesResponseType(typeof(ErrorModel), 409)]
        [ProducesResponseType(typeof(ErrorModel), 500)]
        public async Task<IActionResult> AddRating([FromBody] RatingDTO ratingDTO)
        {
            try
            {
                var result = await _ratingService.AddRating(ratingDTO);
                return Ok(result);
            }
            catch (NoSuchUserExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (NoSuchSongExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (AlreadyRatedException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(409, new ErrorModel(409, ex.Message));
            }
            catch (UnableToAddRatingException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Updates an existing rating for a song.
        /// </summary>
        /// <param name="ratingDTO">The updated rating details.</param>
        /// <returns>The updated rating details.</returns>
        [HttpPut]
        [Authorize]
        [ProducesResponseType(typeof(RatingDTO), 200)]
        [ProducesResponseType(typeof(ErrorModel), 404)]
        [ProducesResponseType(typeof(ErrorModel), 500)]
        public async Task<IActionResult> UpdateRating([FromBody] RatingDTO ratingDTO)
        {
            try
            {
                var result = await _ratingService.UpdateRating(ratingDTO);
                return Ok(result);
            }
            catch (NoSuchUserExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (NoSuchSongExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (NoSuchRatingExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (UnableToUpdateRatingException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves ratings for a specific song.
        /// </summary>
        /// <param name="songId">The ID of the song.</param>
        /// <returns>A list of ratings for the specified song.</returns>
        [HttpGet("song/{songId}")]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<RatingDTO>), 200)]
        [ProducesResponseType(typeof(ErrorModel), 404)]
        [ProducesResponseType(typeof(ErrorModel), 500)]
        public async Task<IActionResult> GetRatingsBySongId(int songId)
        {
            try
            {
                var result = await _ratingService.GetRatingsBySongId(songId);
                return Ok(result);
            }
            catch (NoSuchSongExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (NoRatingsExistsException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves ratings given by a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of ratings given by the specified user.</returns>
        [HttpGet("user/{userId}")]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<RatingDTO>), 200)]
        [ProducesResponseType(typeof(ErrorModel), 404)]
        [ProducesResponseType(typeof(ErrorModel), 500)]
        public async Task<IActionResult> GetRatingsByUserId(int userId)
        {
            try
            {
                var result = await _ratingService.GetRatingsByUserId(userId);
                return Ok(result);
            }
            catch (NoSuchUserExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (NoRatingsExistsException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Deletes a rating for a song by a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="songId">The ID of the song.</param>
        [HttpDelete]
        [Authorize]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(ErrorModel), 404)]
        [ProducesResponseType(typeof(ErrorModel), 500)]
        public async Task<IActionResult> DeleteRating([FromQuery] int userId, [FromQuery] int songId)
        {
            try
            {
                await _ratingService.DeleteRating(userId, songId);
                return Ok("Success");
            }
            catch (NoSuchUserExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (NoSuchSongExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (NoSuchRatingExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (UnableToDeleteRatingException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves the top-rated songs.
        /// </summary>
        /// <returns>A list of top-rated songs with their average ratings.</returns>
        [HttpGet("top-rated")]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<SongRatingDTO>), 200)]
        [ProducesResponseType(typeof(ErrorModel), 500)]
        public async Task<IActionResult> TopRatedSongs()
        {
            try
            {
                var result = await _ratingService.TopRatedSongs();
                return Ok(result);
            }
            catch (UnableToRetrieveTopRatedSongsException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        #endregion
    }
}
