using AutoMapper;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Models.DTOs.SongDTO;
using MusicApplicationAPI.Exceptions.SongExceptions;
using MusicApplicationAPI.Models.Enums;

namespace MusicApplicationAPI.Services.SongService
{
    /// <summary>
    /// Service class to handle song-related operations.
    /// </summary>
    public class SongService : ISongService
    {
        #region Fields
        private readonly ISongRepository _songRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SongService> _logger;
        #endregion

        #region Constructor
        public SongService(ISongRepository songRepository, IMapper mapper, ILogger<SongService> logger)
        {
            _songRepository = songRepository;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Public Methods

        public async Task<SongReturnDTO> AddSong(SongAddDTO songAddDTO)
        {
            try
            {
                var song = _mapper.Map<Song>(songAddDTO);
                song.ReleaseDate = DateTime.Now;
                var addedSong = await _songRepository.Add(song);
                return _mapper.Map<SongReturnDTO>(addedSong);
            }
            catch (UnableToAddSongException ex)
            {
                _logger.LogError(ex, "Unable to add a new song.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new song.");
                throw;
            }
        }

        public async Task<SongReturnDTO> UpdateSong(int songId, SongUpdateDTO songUpdateDTO)
        {
            try
            {
                var song = await _songRepository.GetById(songId);

                song.Title = songUpdateDTO.Title;
                song.Genre = songUpdateDTO.Genre;
                song.ArtistId = songUpdateDTO.ArtistId;
                song.AlbumId = songUpdateDTO.AlbumId;

                var updatedSong = await _songRepository.Update(song);
                return _mapper.Map<SongReturnDTO>(updatedSong);
            }
            catch (NoSuchSongExistException ex)
            {
                _logger.LogError(ex, $"Song not found. {ex}");
                throw;
            }
            catch (UnableToUpdateSongException ex)
            {
                _logger.LogError(ex, "Unable to update a song.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating song with ID {songId}.");
                throw;
            }
        }

        public async Task<SongReturnDTO> GetSongById(int songId)
        {
            try
            {
                var song = await _songRepository.GetById(songId);

                return _mapper.Map<SongReturnDTO>(song);
            }
            catch (NoSuchSongExistException ex)
            {
                _logger.LogError(ex, $"Song not found. {ex}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving song with ID {songId}.");
                throw;
            }
        }

        public async Task<IEnumerable<SongReturnDTO>> GetAllSongs()
        {
            try
            {
                var songs = await _songRepository.GetAll();
                if (songs == null || !songs.Any())
                    throw new NoSongsExistsException("No songs found in the database.");

                return _mapper.Map<IEnumerable<SongReturnDTO>>(songs);
            }
            catch (NoSongsExistsException ex)
            {
                _logger.LogError(ex, "No songs found.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all songs.");
                throw;
            }
        }

        public async Task<SongReturnDTO> DeleteSong(int songId)
        {
            try
            {
                var song = await _songRepository.Delete(songId);

                return _mapper.Map<SongReturnDTO>(song);
            }
            catch (NoSuchSongExistException ex)
            {
                _logger.LogError(ex, $"Song not found. {ex}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting song with ID {songId}.");
                throw;
            }
        }

        public async Task<IEnumerable<SongReturnDTO>> GetSongsByArtistId(int artistId)
        {
            try
            {
                var songs = await _songRepository.GetSongsByArtistId(artistId);
                return _mapper.Map<IEnumerable<SongReturnDTO>>(songs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving songs by artist ID {artistId}.");
                throw;
            }
        }

        public async Task<IEnumerable<SongReturnDTO>> GetSongsByAlbumId(int albumId)
        {
            try
            {
                var songs = await _songRepository.GetSongsByAlbumId(albumId);
                return _mapper.Map<IEnumerable<SongReturnDTO>>(songs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving songs by album ID {albumId}.");
                throw;
            }
        }

        public async Task<IEnumerable<SongReturnDTO>> GetAlbumSongs()
        {
            try
            {
                // Retrieve all songs from the repository
                var songs = (await _songRepository.GetAll()).ToList();

                if (songs.Count == 0)
                    throw new NoSongsExistsException();

                // Filter out songs where AlbumId is null and order by AlbumId
                var filteredSongs = songs
                    .Where(song => song.AlbumId != null)
                    .OrderBy(song => song.AlbumId);

                // Map to DTOs and return
                return _mapper.Map<IEnumerable<SongReturnDTO>>(filteredSongs);
            }
            catch (NoSongsExistsException ex)
            {
                _logger.LogError(ex, "Error retrieving album songs.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving album songs.");
                throw;
            }
        }

        public async Task<IEnumerable<SongReturnDTO>> GetSongsByGenre(string genre)
        {
            try
            {
                // Retrieve all songs from the repository
                var songs = (await _songRepository.GetAll()).ToList();
                Enum.TryParse<GenreType>(genre, true, out var genreTypeEnum);
                if (songs.Count == 0)
                    throw new NoSongsExistsException();

                // Filter songs by the specified genre
                
                var filteredSongs = songs
                    .Where(song => song.Genre == genreTypeEnum);

                // Map to DTOs and return
                return _mapper.Map<IEnumerable<SongReturnDTO>>(filteredSongs);
            }
            catch (NoSongsExistsException ex)
            {
                _logger.LogError(ex, "Error retrieving album songs.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving songs by genre: {genre}.");
                throw;
            }
        }


        #endregion
    }
}
