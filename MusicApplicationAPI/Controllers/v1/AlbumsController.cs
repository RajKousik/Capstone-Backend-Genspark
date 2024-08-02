using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicApplicationAPI.Exceptions.AlbumExceptions;
using MusicApplicationAPI.Exceptions.ArtistExceptions;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Models.DTOs.AlbumDTO;
using MusicApplicationAPI.Models.ErrorModels;
using MusicApplicationAPI.Services.SongService;
using WatchDog;

namespace MusicApplicationAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v1/albums")]
    public class AlbumsController : ControllerBase
    {
        #region Private Fields
        private readonly IAlbumService _albumService;
        private readonly IMapper _mapper;
        private readonly ILogger<AlbumsController> _logger;
        #endregion

        #region Constructor
        public AlbumsController(IAlbumService albumService, IMapper mapper, ILogger<AlbumsController> logger)
        {
            _albumService = albumService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Public Endpoints 

        /// <summary>
        /// Adds a new album.
        /// </summary>
        /// <param name="albumAddDTO">The album data transfer object containing album details.</param>
        /// <returns>The added album data transfer object.</returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(AlbumReturnDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddAlbum([FromBody] AlbumAddDTO albumAddDTO)
        {
            try
            {
                var album = await _albumService.AddAlbum(albumAddDTO);
                return CreatedAtAction(nameof(GetAlbumById), new { albumId = album.AlbumId }, album);
            }
            catch (NoSuchArtistExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (UnableToAddAlbumException ex)
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
        /// Updates an existing album.
        /// </summary>
        /// <param name="albumId">The ID of the album to update.</param>
        /// <param name="albumUpdateDTO">The album data transfer object containing updated album details.</param>
        /// <returns>The updated album data transfer object.</returns>
        [HttpPut("{albumId}")]
        [Authorize]
        [ProducesResponseType(typeof(AlbumReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAlbum(int albumId, [FromBody] AlbumUpdateDTO albumUpdateDTO)
        {
            try
            {
                var album = await _albumService.UpdateAlbum(albumId, albumUpdateDTO);
                return Ok(album);
            }
            catch (NoSuchAlbumExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (UnableToUpdateAlbumException ex)
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


        [Authorize]
        [HttpDelete("range")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> DeleteAlbum([FromBody] IList<int> albumIds)
        {
            try
            {
                var result = await _albumService.DeleteRangeAlbums(albumIds);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message, ex);
                return NotFound(new ErrorModel(404, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves an album by its ID.
        /// </summary>
        /// <param name="albumId">The ID of the album to retrieve.</param>
        /// <returns>The album data transfer object.</returns>
        [HttpGet("{albumId}")]
        [Authorize]
        [ProducesResponseType(typeof(AlbumReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAlbumById(int albumId)
        {
            try
            {
                var album = await _albumService.GetAlbumById(albumId);
                return Ok(album);
            }
            catch (NoSuchAlbumExistException ex)
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
        /// Retrieves all albums.
        /// </summary>
        /// <returns>A list of album data transfer objects.</returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<AlbumReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAlbums()
        {
            try
            {
                var albums = await _albumService.GetAllAlbums();
                return Ok(albums);
            }
            catch (NoAlbumsExistsException ex)
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
        /// Retrieves albums by artist ID.
        /// </summary>
        /// <param name="artistId">The artist ID to filter albums by.</param>
        /// <returns>A list of album data transfer objects.</returns>
        [HttpGet("artist/{artistId}")]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<AlbumReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAlbumsByArtistId(int artistId)
        {
            try
            {
                var albums = await _albumService.GetAlbumsByArtistId(artistId);
                return Ok(albums);
            }
            catch (NoSuchArtistExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (NoAlbumsExistsException ex)
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
        /// Deletes an album by its ID.
        /// </summary>
        /// <param name="albumId">The ID of the album to delete.</param>
        /// <returns>The deleted album data transfer object.</returns>
        [HttpDelete("{albumId}")]
        [Authorize]
        [ProducesResponseType(typeof(AlbumReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAlbum(int albumId)
        {
            try
            {
                var deletedAlbum = await _albumService.DeleteAlbum(albumId);
                return Ok(deletedAlbum);
            }
            catch (NoSuchAlbumExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (UnableToDeleteAlbumException ex)
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
