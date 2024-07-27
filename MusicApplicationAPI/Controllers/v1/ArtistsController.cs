using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicApplicationAPI.Exceptions.ArtistExceptions;
using MusicApplicationAPI.Exceptions.SongExceptions;
using MusicApplicationAPI.Exceptions.UserExceptions;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Models.DTOs.ArtistDTO;
using MusicApplicationAPI.Models.DTOs.OtherDTO;
using MusicApplicationAPI.Models.DTOs.SongDTO;
using MusicApplicationAPI.Models.ErrorModels;
using WatchDog;

namespace MusicApplicationAPI.Controllers.v1
{
    [Route("api/v1/artists")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        #region Private Fields
        private readonly IArtistService _artistService;
        private readonly ILogger<ArtistsController> _logger;
        #endregion

        #region Constructor
        public ArtistsController(IArtistService artistService, ILogger<ArtistsController> logger)
        {
            _artistService = artistService;
            _logger = logger;
        }
        #endregion

        #region Public Endpoints

        /// <summary>
        /// Updates an existing artist.
        /// </summary>
        /// <param name="artistId">The ID of the artist to update.</param>
        /// <param name="artistUpdateDTO">The updated artist data.</param>
        /// <returns>The updated artist as a DTO.</returns>
        [HttpPut("{artistId:int}")]
        [Authorize(Roles = "Admin, Artist")]
        [ProducesResponseType(typeof(ArtistReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateArtist(int artistId, [FromBody] ArtistUpdateDTO artistUpdateDTO)
        {
            try
            {
                var updatedArtist = await _artistService.UpdateArtist(artistId, artistUpdateDTO);
                return Ok(updatedArtist);
            }
            catch (NoSuchArtistExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (UnableToDeleteArtistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, "Error updating artist.");
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves an artist by ID.
        /// </summary>
        /// <param name="artistId">The ID of the artist to retrieve.</param>
        /// <returns>The artist as a DTO.</returns>
        [HttpGet("{artistId:int}")]
        [Authorize]
        [ProducesResponseType(typeof(ArtistReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetArtistById(int artistId)
        {
            try
            {
                var artist = await _artistService.GetArtistById(artistId);
                return Ok(artist);
            }
            catch (NoSuchArtistExistException ex)
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
        /// Retrieves all artists.
        /// </summary>
        /// <returns>A list of all artists as DTOs.</returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(List<ArtistReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllArtists()
        {
            try
            {
                var artists = await _artistService.GetAllArtists();
                return Ok(artists);
            }
            catch (NoArtistsExistsException ex)
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
        /// Changes the password for an artist.
        /// </summary>
        /// <param name="requestDTO">The request data for changing the password.</param>
        /// <param name="artistId">The ID of the artist whose password is being changed.</param>
        /// <returns>Success message or error details.</returns>
        [Authorize]
        [HttpPut("change-password")]
        [Authorize(Roles = "Admin, Artist")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDTO requestDTO, int artistId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _artistService.ChangePassword(requestDTO, artistId);
                if (result)
                    return Ok(new { message = "Password changed successfully." });
                else
                    return BadRequest(new { message = "Current password is incorrect." });
            }
            catch (NoSuchArtistExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (InvalidPasswordException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(400, new ErrorModel(400, ex.Message));
            }
            catch (UnableToUpdateArtistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Deletes an artist by ID.
        /// </summary>
        /// <param name="artistId">The ID of the artist to delete.</param>
        /// <returns>The deleted artist as a DTO.</returns>
        [HttpDelete("{artistId:int}")]
        [Authorize(Roles = "Admin, Artist")]
        [ProducesResponseType(typeof(ArtistReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteArtist(int artistId)
        {
            try
            {
                var deletedArtist = await _artistService.DeleteArtist(artistId);
                return Ok(deletedArtist);
            }
            catch (NoSuchArtistExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (UnableToDeleteArtistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, "Error deleting artist.");
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves songs by a specific artist ID.
        /// </summary>
        /// <param name="artistId">The ID of the artist.</param>
        /// <returns>A list of songs by the specified artist as DTOs.</returns>
        [HttpGet("{artistId:int}/songs")]
        [Authorize]
        [ProducesResponseType(typeof(List<SongReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSongsByArtist(int artistId)
        {
            try
            {
                var songs = await _artistService.GetSongsByArtist(artistId);
                return Ok(songs);
            }
            catch (NoSuchArtistExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (NoSongsExistsException ex)
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

        #endregion
    }
}
