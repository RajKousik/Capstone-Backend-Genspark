using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicApplicationAPI.Exceptions.PlaylistExceptions;
using MusicApplicationAPI.Exceptions.PlaylistSongExceptions;
using MusicApplicationAPI.Exceptions.SongExceptions;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Models.DTOs.PlaylistSongDTO;
using MusicApplicationAPI.Models.ErrorModels;
using WatchDog;

namespace MusicApplicationAPI.Controllers.v1
{
    [Route("api/v1/playlist-songs")]
    [ApiController]
    public class PlaylistSongsController : ControllerBase
    {
        #region Private Fields
        private readonly IPlaylistSongService _playlistSongService;
        private readonly ILogger<PlaylistSongsController> _logger;
        #endregion

        #region Constructor
        public PlaylistSongsController(IPlaylistSongService playlistSongService, ILogger<PlaylistSongsController> logger)
        {
            _playlistSongService = playlistSongService;
            _logger = logger;
        }
        #endregion

        #region Public Endpoints

        /// <summary>
        /// Adds a song to a playlist.
        /// </summary>
        /// <param name="playlistSongDTO">The data to add a song to a playlist.</param>
        /// <returns>The added song details in the playlist.</returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddSongToPlaylist([FromBody] PlaylistSongDTO playlistSongDTO)
        {
            try
            {
                var result = await _playlistSongService.AddSongToPlaylist(playlistSongDTO);
                return CreatedAtAction(nameof(GetSongsInPlaylist), new { playlistId = result.PlaylistId }, result);
            }
            catch (NoSuchPlaylistExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (NoSuchSongExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (UnableToAddPlaylistSongException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Removes a song from a playlist.
        /// </summary>
        /// <param name="playlistSongDTO">The data to remove a song from a playlist.</param>
        /// <returns>The removed song details from the playlist.</returns>
        [HttpDelete]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveSongFromPlaylist([FromBody] PlaylistSongDTO playlistSongDTO)
        {
            try
            {
                var result = await _playlistSongService.RemoveSongFromPlaylist(playlistSongDTO);
                return Ok(result);
            }
            catch (NoSuchPlaylistExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (NoSuchSongExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (NoSuchPlaylistSongExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (UnableToDeletePlaylistSongException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves all songs in a specific playlist.
        /// </summary>
        /// <param name="playlistId">The ID of the playlist.</param>
        /// <returns>A list of songs in the specified playlist.</returns>
        [HttpGet("{playlistId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSongsInPlaylist(int playlistId)
        {
            try
            {
                var result = await _playlistSongService.GetSongsInPlaylist(playlistId);
                return Ok(result);
            }
            catch (NoSuchPlaylistExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (NoSongsInPlaylistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, "Error retrieving songs in playlist.");
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Clears all songs from a playlist.
        /// </summary>
        /// <param name="playlistId">The ID of the playlist to clear.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        [HttpDelete("clear/{playlistId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ClearPlaylist(int playlistId)
        {
            try
            {
                await _playlistSongService.ClearPlaylist(playlistId);
                return NoContent();
            }
            catch (NoSuchPlaylistExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (NoSongsInPlaylistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (UnableToClearPlaylistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, "Error clearing playlist.");
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Gets the count of songs in a playlist.
        /// </summary>
        /// <param name="playlistId">The ID of the playlist.</param>
        /// <returns>The count of songs in the specified playlist.</returns>
        [HttpGet("count/{playlistId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSongCountInPlaylist(int playlistId)
        {
            try
            {
                var count = await _playlistSongService.GetSongCountInPlaylist(playlistId);
                return Ok(count);
            }
            catch (NoSuchPlaylistExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
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
