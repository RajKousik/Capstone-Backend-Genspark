using AutoMapper;
using MusicApplicationAPI.Exceptions.AlbumExceptions;
using MusicApplicationAPI.Exceptions.ArtistExceptions;
using MusicApplicationAPI.Exceptions.SongExceptions;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Models.DTOs.AlbumDTO;
using MusicApplicationAPI.Repositories;

namespace MusicApplicationAPI.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly ISongRepository _songRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AlbumService> _logger;

        public AlbumService(IAlbumRepository albumRepository, IArtistRepository artistRepository, IMapper mapper, ILogger<AlbumService> logger, ISongRepository songRepository)
        {
            _albumRepository = albumRepository;
            _artistRepository = artistRepository;
            _mapper = mapper;
            _logger = logger;
            _songRepository = songRepository;
        }

        /// <summary>
        /// Adds a new album to the system.
        /// </summary>
        /// <param name="albumAddDTO">The album data transfer object containing album details.</param>
        /// <returns>The added album data transfer object.</returns>
        /// <exception cref="UnableToAddAlbumException">Thrown when the album could not be added.</exception>
        public async Task<AlbumReturnDTO> AddAlbum(AlbumAddDTO albumAddDTO)
        {
            try
            {
                // Validate artist existence
                var artist = await _artistRepository.GetById(albumAddDTO.ArtistId);
                if (artist == null)
                    throw new NoSuchArtistExistException($"Artist with ID {albumAddDTO.ArtistId} does not exist.");

                var album = _mapper.Map<Album>(albumAddDTO);
                album.ReleaseDate = DateTime.UtcNow;

                var addedAlbum = await _albumRepository.Add(album);
                return _mapper.Map<AlbumReturnDTO>(addedAlbum);
            }
            catch (NoSuchArtistExistException ex)
            {
                _logger.LogError(ex, $"Artist with ID {albumAddDTO.ArtistId} does not exist.");
                throw;
            }
            catch (UnableToAddAlbumException ex)
            {
                _logger.LogError(ex, $"Unable to add album.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new album.");
                throw;
            }
        }



        public async Task<bool> DeleteRangeAlbums(IList<int> ids)
        {
            try
            {
                await _albumRepository.DeleteRange(ids);
                return true;
            }
            catch (NoAlbumsExistsException ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Unable to delete albums" + ex.Message);
                throw new NoSuchAlbumExistException("Unable to delete albums. " + ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing album in the system.
        /// </summary>
        /// <param name="albumId">The ID of the album to update.</param>
        /// <param name="albumUpdateDTO">The album data transfer object containing updated album details.</param>
        /// <returns>The updated album data transfer object.</returns>
        /// <exception cref="NoSuchAlbumExistException">Thrown when the specified album does not exist.</exception>
        /// <exception cref="UnableToUpdateAlbumException">Thrown when the album could not be updated.</exception>
        public async Task<AlbumReturnDTO> UpdateAlbum(int albumId, AlbumUpdateDTO albumUpdateDTO)
        {
            try
            {
                var album = await _albumRepository.GetById(albumId);
                if (album == null)
                    throw new NoSuchAlbumExistException($"Album with ID {albumId} does not exist.");

                _mapper.Map(albumUpdateDTO, album);
                var updatedAlbum = await _albumRepository.Update(album);
                return _mapper.Map<AlbumReturnDTO>(updatedAlbum);
            }
            catch (NoSuchAlbumExistException ex)
            {
                _logger.LogError(ex, $"Album with ID {albumId} does not exist.");
                throw;
            }
            catch (UnableToUpdateAlbumException ex)
            {
                _logger.LogError(ex, $"Unable to update album.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating album with ID {albumId}.");
                throw;
            }
        }

        /// <summary>
        /// Retrieves an album by its ID.
        /// </summary>
        /// <param name="albumId">The ID of the album to retrieve.</param>
        /// <returns>The album data transfer object.</returns>
        /// <exception cref="NoSuchAlbumExistException">Thrown when the specified album does not exist.</exception>
        public async Task<AlbumReturnDTO> GetAlbumById(int albumId)
        {
            try
            {
                var album = await _albumRepository.GetById(albumId);
                if (album == null)
                    throw new NoSuchAlbumExistException($"Album with ID {albumId} does not exist.");

                return _mapper.Map<AlbumReturnDTO>(album);
            }
            catch (NoSuchAlbumExistException ex)
            {
                _logger.LogError(ex, $"Album with ID {albumId} does not exist.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving album with ID {albumId}.");
                throw;
            }
        }

        /// <summary>
        /// Retrieves all albums in the system.
        /// </summary>
        /// <returns>A list of album data transfer objects.</returns>
        /// <exception cref="NoSuchAlbumExistException">Thrown when no albums exist.</exception>
        public async Task<IEnumerable<AlbumReturnDTO>> GetAllAlbums()
        {
            try
            {
                var albums = (await _albumRepository.GetAll()).ToList();
                if (albums.Count == 0)
                    throw new NoAlbumsExistsException("No albums found.");

                return _mapper.Map<IEnumerable<AlbumReturnDTO>>(albums);
            }
            catch (NoAlbumsExistsException ex)
            {
                _logger.LogError(ex, $"No Albums found.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all albums.");
                throw;
            }
        }

        /// <summary>
        /// Retrieves albums by a specific artist ID.
        /// </summary>
        /// <param name="artistId">The artist ID to filter albums by.</param>
        /// <returns>A list of album data transfer objects.</returns>
        /// <exception cref="NoSuchAlbumExistException">Thrown when no albums exist for the specified artist.</exception>
        public async Task<IEnumerable<AlbumReturnDTO>> GetAlbumsByArtistId(int artistId)
        {
            try
            {
                await _artistRepository.GetById(artistId);

                var albums = (await _albumRepository.GetAll()).Where(a => a.ArtistId == artistId).ToList();
                if (albums.Count == 0)
                    throw new NoAlbumsExistsException($"No albums found for artist with ID {artistId}.");

                return _mapper.Map<IEnumerable<AlbumReturnDTO>>(albums);
            }
            catch (NoSuchArtistExistException ex)
            {
                _logger.LogError(ex, $"Artist with ID {artistId} does not exist.");
                throw;
            }
            catch (NoAlbumsExistsException ex)
            {
                _logger.LogError(ex, $"No albums found for artist with ID {artistId}.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving albums for artist ID {artistId}.");
                throw;
            }
        }

        /// <summary>
        /// Deletes an album by its ID.
        /// </summary>
        /// <param name="albumId">The ID of the album to delete.</param>
        /// <returns>The deleted album data transfer object.</returns>
        /// <exception cref="NoSuchAlbumExistException">Thrown when the specified album does not exist.</exception>
        /// <exception cref="UnableToDeleteAlbumException">Thrown when the album could not be deleted.</exception>
        public async Task<AlbumReturnDTO> DeleteAlbum(int albumId)
        {
            try
            {
                var album = await _albumRepository.GetById(albumId);
                if (album == null)
                    throw new NoSuchAlbumExistException($"Album with ID {albumId} does not exist.");

                await DeleteRelatedSongs(albumId);
                var deletedAlbum = await _albumRepository.Delete(albumId);
                return _mapper.Map<AlbumReturnDTO>(deletedAlbum);
            }
            catch (UnableToDeleteAlbumException ex)
            {
                _logger.LogError(ex, $"Unable to delete album.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting album with ID {albumId}.");
                throw;
            }
        }

        #region Private Methods
        private async Task DeleteRelatedSongs(int albumId)
        {
            try
            {
                var songs = (await _songRepository.GetAll()).Where(s => s.AlbumId == albumId).ToList();

                if (songs.Count > 0)
                {
                    foreach (var song in songs)
                    {
                        await _songRepository.Delete(song.SongId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting related songs.");
                throw new UnableToDeleteAlbumException(ex.Message);
            }
        }
        #endregion
    }
}
