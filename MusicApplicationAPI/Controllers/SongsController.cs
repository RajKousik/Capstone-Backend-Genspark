using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicApplicationAPI.Exceptions.AlbumExceptions;
using MusicApplicationAPI.Exceptions.ArtistExceptions;
using MusicApplicationAPI.Exceptions.SongExceptions;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Models.DTOs.SongDTO;
using MusicApplicationAPI.Models.ErrorModels;
using WatchDog;

namespace MusicApplicationAPI.Controllers
{
    [Route("api/v1/songs")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        #region Private Fields

        private readonly ISongService _songService;
        private readonly ILogger<SongsController> _logger;

        #endregion

        #region Constructor

        public SongsController(ISongService songService, ILogger<SongsController> logger)
        {
            _songService = songService;
            _logger = logger;
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Adds a new song.
        /// </summary>
        /// <param name="songDto">The song data to add.</param>
        /// <returns>The added song data.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(SongReturnDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddSong([FromBody] SongAddDTO songDto)
        {
            if (songDto == null)
            {
                return BadRequest(new ErrorModel(400, "Invalid song data."));
            }

            try
            {
                var result = await _songService.AddSong(songDto);
                return CreatedAtAction(nameof(GetSongById), new { songId = result.SongId }, result);
            }
            catch (NoSuchAlbumExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(404, $"{ex.Message}"));
            }
            catch (NoSuchArtistExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(400, $"{ex.Message}"));
            }
            catch (InvalidSongDuration ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(400, $"{ex.Message}"));
            }
            catch (InvalidGenreException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(404, $"{ex.Message}"));
            }
            catch (UnableToAddSongException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorModel(500, $"{ex.Message}"));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorModel(500, $"{ex.Message}"));
            }
        }

        /// <summary>
        /// Retrieves a song by its ID.
        /// </summary>
        /// <param name="songId">The ID of the song to retrieve.</param>
        /// <returns>The song data.</returns>
        [HttpGet("{songId}")]
        [ProducesResponseType(typeof(SongReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSongById([FromRoute] int songId)
        {
            if (songId <= 0)
            {
                return BadRequest(new ErrorModel(400, "Invalid song ID."));
            }

            try
            {
                var result = await _songService.GetSongById(songId);
                return Ok(result);
            }
            catch (NoSuchSongExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Updates a song's details.
        /// </summary>
        /// <param name="songDto">The song data to update.</param>
        /// <param name="songId">The ID of the song to update.</param>
        /// <returns>The updated song data.</returns>
        [HttpPut("{songId}")]
        [ProducesResponseType(typeof(SongReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateSong([FromBody] SongUpdateDTO songDto, [FromRoute] int songId)
        {
            if (songDto == null || songId <= 0)
            {
                return BadRequest(new ErrorModel(400, "Invalid song data or song ID."));
            }

            try
            {
                var result = await _songService.UpdateSong(songId, songDto);
                return Ok(result);
            }
            catch (NoSuchSongExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (NoSuchAlbumExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(404, $"{ex.Message}"));
            }
            catch (NoSuchArtistExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(404, $"{ex.Message}"));
            }
            catch (InvalidSongDuration ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(400, $"{ex.Message}"));
            }
            catch (InvalidGenreException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(400, $"{ex.Message}"));
            }
            catch (UnableToUpdateSongException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorModel(500, $"{ex.Message}"));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Deletes a song by its ID.
        /// </summary>
        /// <param name="songId">The ID of the song to delete.</param>
        /// <returns>A message indicating the result of the deletion.</returns>
        [HttpDelete("{songId}")]
        [ProducesResponseType(typeof(SongReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteSong([FromRoute] int songId)
        {
            if (songId <= 0)
            {
                return BadRequest(new ErrorModel(400, "Invalid song ID."));
            }

            try
            {
                var result = await _songService.DeleteSong(songId);
                return Ok(result);
            }
            catch (NoSuchSongExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (UnableToDeleteSongException ex)
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
        /// Retrieves songs by genre.
        /// </summary>
        /// <param name="genre">The genre to filter songs by.</param>
        /// <returns>A list of songs in the specified genre.</returns>
        [HttpGet("genre")]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<SongReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSongsByGenre([FromQuery] string genre)
        {
            if (string.IsNullOrEmpty(genre))
            {
                return BadRequest(new ErrorModel(400, "Genre is required."));
            }

            try
            {
                var result = await _songService.GetSongsByGenre(genre);
                return Ok(result);
            }
            catch (NoSuchSongExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (InvalidGenreException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(400, ex.Message));
            }
            catch (NoSongsExistsException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves songs by artist.
        /// </summary>
        /// <param name="artistId">The artist Id to filter songs by.</param>
        /// <returns>A list of songs by the specified artist.</returns>
        [HttpGet("artist/{artistId}")]
        [ProducesResponseType(typeof(IEnumerable<SongReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSongsByArtist([FromRoute] int artistId)
        {
            if (artistId <= 0)
            {
                return BadRequest(new ErrorModel(400, "Artist Id is required."));
            }

            try
            {
                var result = await _songService.GetSongsByArtistId(artistId);
                return Ok(result);
            }
            catch (NoSuchArtistExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (NoSongsExistsException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves all songs.
        /// </summary>
        /// <returns>A list of all songs.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SongReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllSongs()
        {
            try
            {
                var result = await _songService.GetAllSongs();
                return Ok(result);
            }
            catch (NoSongsExistsException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves songs by album ID.
        /// </summary>
        /// <param name="albumId">The ID of the album to retrieve songs for.</param>
        /// <returns>A list of songs in the specified album.</returns>
        [HttpGet("album/{albumId}")]
        [ProducesResponseType(typeof(IEnumerable<SongReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSongsByAlbumId([FromRoute] int albumId)
        {
            if (albumId <= 0)
            {
                return BadRequest(new ErrorModel(400, "Invalid album ID."));
            }

            try
            {
                var result = await _songService.GetSongsByAlbumId(albumId);
                return Ok(result);
            }
            catch (NoSuchAlbumExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (NoSongsExistsException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves all songs in an album.
        /// </summary>
        /// <returns>A list of album songs.</returns>
        [HttpGet("album-songs")]
        [ProducesResponseType(typeof(IEnumerable<SongReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAlbumSongs()
        {

            try
            {
                var result = await _songService.GetAlbumSongs();
                return Ok(result);
            }
            catch (NoSuchAlbumExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (NoSongsExistsException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        #endregion
    }
}
