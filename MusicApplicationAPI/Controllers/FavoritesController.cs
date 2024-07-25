using Microsoft.AspNetCore.Mvc;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Models.DTOs.FavoriteDTO;
using MusicApplicationAPI.Models.ErrorModels;
using MusicApplicationAPI.Exceptions.FavoriteExceptions;
using MusicApplicationAPI.Exceptions.SongExceptions;
using MusicApplicationAPI.Exceptions.UserExceptions;
using MusicApplicationAPI.Exceptions.PlaylistExceptions;
using WatchDog;
using MusicApplicationAPI.Models.DbModels;

namespace MusicApplicationAPI.Controllers
{
    [ApiController]
    [Route("api/v1/favorites")]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;
        private readonly ILogger<FavoritesController> _logger;

        public FavoritesController(IFavoriteService favoriteService, ILogger<FavoritesController> logger)
        {
            _favoriteService = favoriteService;
            _logger = logger;
        }

        /// <summary>
        /// Marks a song as a favorite for a user.
        /// </summary>
        /// <param name="favoriteDTO">The favorite data transfer object containing user and song information.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpPost("song")]
        public async Task<IActionResult> MarkSongAsFavorite([FromBody] FavoriteSongDTO favoriteSongDTO)
        {
            try
            {
                await _favoriteService.MarkSongAsFavorite(favoriteSongDTO);
                return StatusCode(201, "success");
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
            catch (AlreadyMarkedAsFavorite ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(409, new ErrorModel(409, ex.Message));
            }
            catch (UnableToAddFavoriteException ex)
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
        /// Removes a song from a user's favorites.
        /// </summary>
        /// <param name="favoriteDTO">The favorite data transfer object containing user and song information.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpDelete("song")]
        public async Task<IActionResult> RemoveSongFromFavorites([FromBody] FavoriteSongDTO favoriteSongDTO)
        {
            try
            {
                await _favoriteService.RemoveSongFromFavorites(favoriteSongDTO);
                return StatusCode(201, "success");
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
            catch (NotMarkedAsFavorite ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(409, new ErrorModel(409, ex.Message));
            }
            catch (UnableToDeleteFavoriteException ex)
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
        /// Marks a playlist as a favorite for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="playlistId">The ID of the playlist.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpPost("playlist")]
        public async Task<IActionResult> MarkPlaylistAsFavorite([FromBody] FavoritePlaylistDTO favoritePlaylistDTO)
        {
            try
            {
                await _favoriteService.MarkPlaylistAsFavorite(favoritePlaylistDTO);
                return StatusCode(201, "success");
            }
            catch (NoSuchUserExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (NoSuchPlaylistExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (AlreadyMarkedAsFavorite ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(409, new ErrorModel(409, ex.Message));
            }
            catch (UnableToAddFavoriteException ex)
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
        /// Removes a playlist from a user's favorites.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="playlistId">The ID of the playlist.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpDelete("playlist")]
        public async Task<IActionResult> RemovePlaylistFromFavorites([FromBody] FavoritePlaylistDTO favoritePlaylistDTO)
        {
            try
            {
                await _favoriteService.RemovePlaylistFromFavorites(favoritePlaylistDTO);
                return StatusCode(201, "success");
            }
            catch (NoSuchUserExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (NoSuchPlaylistExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (NotMarkedAsFavorite ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(409, new ErrorModel(409, ex.Message));
            }
            catch (UnableToDeleteFavoriteException ex)
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
        /// Retrieves a user's favorite songs.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of favorite songs for the specified user.</returns>
        [HttpGet("songs")]
        public async Task<IActionResult> GetFavoriteSongsByUserId([FromQuery] int userId)
        {
            try
            {
                var favoriteSongs = await _favoriteService.GetFavoriteSongsByUserId(userId);
                return Ok(favoriteSongs);
            }
            catch (NoSuchUserExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (NoFavoritesExistsException ex)
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
        /// Retrieves a user's favorite playlists.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of favorite playlists for the specified user.</returns>
        [HttpGet("playlists")]
        public async Task<IActionResult> GetFavoritePlaylistsByUserId([FromQuery] int userId)
        {
            try
            {
                var favoritePlaylists = await _favoriteService.GetFavoritePlaylistsByUserId(userId);
                return Ok(favoritePlaylists);
            }
            catch (NoSuchUserExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (NoFavoritesExistsException ex)
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
