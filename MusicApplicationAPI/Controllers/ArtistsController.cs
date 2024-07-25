using Microsoft.AspNetCore.Mvc;
using MusicApplicationAPI.Exceptions.ArtistExceptions;
using MusicApplicationAPI.Exceptions.SongExceptions;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Models.DTOs.ArtistDTO;
using MusicApplicationAPI.Models.ErrorModels;
using WatchDog;

namespace MusicApplicationAPI.Controllers
{
    [Route("api/v1/artists")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly IArtistService _artistService;
        private readonly ILogger<ArtistsController> _logger;

        public ArtistsController(IArtistService artistService, ILogger<ArtistsController> logger)
        {
            _artistService = artistService;
            _logger = logger;
        }

        /// <summary>
        /// Adds a new artist.
        /// </summary>
        /// <param name="artistAddDTO">The artist data to add.</param>
        /// <returns>The added artist as a DTO.</returns>
        [HttpPost]
        public async Task<IActionResult> AddArtist([FromBody] ArtistAddDTO artistAddDTO)
        {
            try
            {
                var addedArtist = await _artistService.AddArtist(artistAddDTO);
                return CreatedAtAction(nameof(GetArtistById), new { artistId = addedArtist.ArtistId }, addedArtist);
            }
            catch (UnableToAddArtistException ex)
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
        /// Updates an existing artist.
        /// </summary>
        /// <param name="artistId">The ID of the artist to update.</param>
        /// <param name="artistUpdateDTO">The updated artist data.</param>
        /// <returns>The updated artist as a DTO.</returns>
        [HttpPut("{artistId:int}")]
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
        /// Deletes an artist by ID.
        /// </summary>
        /// <param name="artistId">The ID of the artist to delete.</param>
        /// <returns>The deleted artist as a DTO.</returns>
        [HttpDelete("{artistId:int}")]
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
    }
}
