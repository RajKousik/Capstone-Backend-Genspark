using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicApplicationAPI.Exceptions.PlaylistExceptions;
using MusicApplicationAPI.Exceptions.UserExceptions;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Models.DTOs.PlaylistDTO;
using MusicApplicationAPI.Models.ErrorModels;
using WatchDog;

namespace MusicApplicationAPI.Controllers.v1
{
    [Route("api/v1/playlists")]
    [ApiController]
    public class PlaylistsController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;
        private readonly ILogger<PlaylistsController> _logger;

        public PlaylistsController(IPlaylistService playlistService, ILogger<PlaylistsController> logger)
        {
            _playlistService = playlistService;
            _logger = logger;
        }

        /// <summary>
        /// Adds a new playlist.
        /// </summary>
        /// <param name="playlistCreateDTO">The playlist data to be added.</param>
        /// <returns>The added playlist data.</returns>
        [HttpPost]
        public async Task<IActionResult> AddPlaylist([FromBody] PlaylistAddDTO playlistCreateDTO)
        {
            try
            {
                var result = await _playlistService.AddPlaylist(playlistCreateDTO);
                return CreatedAtAction(nameof(GetPlaylistById), new { playlistId = result.PlaylistId }, result);
            }
            catch (NoSuchUserExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (MaximumPlaylistsReachedException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(400, new ErrorModel(400, ex.Message));
            }
            catch (UnableToAddPlaylistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
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
        /// Updates an existing playlist.
        /// </summary>
        /// <param name="playlistId">The ID of the playlist to be updated.</param>
        /// <param name="playlistUpdateDTO">The updated playlist data.</param>
        /// <returns>The updated playlist data.</returns>
        [HttpPut("{playlistId}")]
        public async Task<IActionResult> UpdatePlaylist(int playlistId, [FromBody] PlaylistUpdateDTO playlistUpdateDTO)
        {
            try
            {
                var result = await _playlistService.UpdatePlaylist(playlistId, playlistUpdateDTO);
                return Ok(result);
            }
            catch (NoSuchPlaylistExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (UnableToUpdatePlaylistException ex)
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
        /// Retrieves a playlist by its ID.
        /// </summary>
        /// <param name="playlistId">The ID of the playlist to be retrieved.</param>
        /// <returns>The playlist data.</returns>
        [HttpGet("{playlistId}")]
        public async Task<IActionResult> GetPlaylistById(int playlistId)
        {
            try
            {
                var result = await _playlistService.GetPlaylistById(playlistId);
                return Ok(result);
            }
            catch (NoSuchPlaylistExistException ex)
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
        /// Retrieves all playlists.
        /// </summary>
        /// <returns>A list of all playlists.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllPlaylists()
        {
            try
            {
                var result = await _playlistService.GetAllPlaylists();
                return Ok(result);
            }
            catch (NoPlaylistsExistsException ex)
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
        /// Deletes a playlist by its ID.
        /// </summary>
        /// <param name="playlistId">The ID of the playlist to be deleted.</param>
        /// <returns>The deleted playlist data.</returns>
        [HttpDelete("{playlistId}")]
        public async Task<IActionResult> DeletePlaylist(int playlistId)
        {
            try
            {
                var result = await _playlistService.DeletePlaylist(playlistId);
                return Ok(result);
            }
            catch (NoSuchPlaylistExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (UnableToDeletePlaylistException ex)
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
        /// Retrieves playlists by a specific user ID.
        /// </summary>
        /// <param name="userId">The user ID for which to retrieve playlists.</param>
        /// <returns>A list of playlists for the specified user.</returns>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPlaylistsByUserId(int userId)
        {
            try
            {
                var result = await _playlistService.GetPlaylistsByUserId(userId);
                return Ok(result);
            }
            catch (NoSuchUserExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (NoPlaylistsExistsException ex)
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
        /// Retrieves all public playlists.
        /// </summary>
        /// <returns>A list of public playlists.</returns>
        [HttpGet("public")]
        public async Task<IActionResult> GetPublicPlaylists()
        {
            try
            {
                var result = await _playlistService.GetPublicPlaylists();
                return Ok(result);
            }
            catch (NoPlaylistsExistsException ex)
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
