using AutoMapper;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Models.DTOs.SongDTO;
using MusicApplicationAPI.Exceptions.SongExceptions;
using MusicApplicationAPI.Models.Enums;
using MusicApplicationAPI.Exceptions.ArtistExceptions;
using MusicApplicationAPI.Exceptions.AlbumExceptions;
using System.Diagnostics.CodeAnalysis;

namespace MusicApplicationAPI.Services.SongService
{
    /// <summary>
    /// Service class to handle song-related operations.
    /// </summary>
    public class SongService : ISongService
    {
        #region Fields
        private readonly ISongRepository _songRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly IAlbumRepository _albumRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SongService> _logger;
        #endregion

        #region Constructor
        public SongService(ISongRepository songRepository, IMapper mapper, ILogger<SongService> logger, IArtistRepository artistRepository, IAlbumRepository albumRepository)
        {
            _songRepository = songRepository;
            _artistRepository = artistRepository;
            _mapper = mapper;
            _logger = logger;
            _albumRepository = albumRepository;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a new song to the system.
        /// </summary>
        /// <param name="songAddDTO">The song data to add.</param>
        /// <returns>The added song as a DTO.</returns>
        /// <exception cref="NoSuchArtistException">Thrown when the specified artist does not exist.</exception>
        /// <exception cref="NoSuchAlbumException">Thrown when the specified album does not exist.</exception>
        /// <exception cref="UnableToAddSongException">Thrown when the song cannot be added.</exception>
        public async Task<SongReturnDTO> AddSong(SongAddDTO songAddDTO)
        {
            try
            {
                // Check if the artist exists
                var artist = await _artistRepository.GetById(songAddDTO.ArtistId);
                if (artist == null)
                {
                    throw new NoSuchArtistExistException($"Artist with ID {songAddDTO.ArtistId} does not exist.");
                }

                // If album ID is provided, check if the album exists
                if (songAddDTO.AlbumId.HasValue)
                {
                    var album = await _albumRepository.GetById(songAddDTO.AlbumId.Value);
                    if (album == null)
                    {
                        throw new NoSuchAlbumExistException($"Album with ID {songAddDTO.AlbumId.Value} does not exist.");
                    }
                    else
                    {
                        if(songAddDTO.ArtistId != album.ArtistId)
                        {
                            throw new UnableToAddSongException("Song Artist is different from Album's Artist");
                        }
                    }
                }

                if (songAddDTO.Duration <= 0)
                {
                    throw new InvalidSongDuration();
                }

                if (!(Enum.TryParse<GenreType>(songAddDTO.Genre, true, out var genreTypeEnum)))
                {
                    throw new InvalidGenreException();
                }

                    // Map DTO to entity and set the release date
                    Song song = new Song()
                {
                    Url = songAddDTO.Url,
                    Title = songAddDTO.Title,
                    Genre = genreTypeEnum,
                    ArtistId = songAddDTO.ArtistId,
                    AlbumId = songAddDTO.AlbumId,
                    Duration = songAddDTO.Duration,
                    ReleaseDate = DateTime.Now,
                    ImageUrl = songAddDTO.ImageUrl,
                };

                // Add the song to the repository
                var addedSong = await _songRepository.Add(song);
                return _mapper.Map<SongReturnDTO>(addedSong);
            }
            catch (NoSuchArtistExistException ex)
            {
                _logger.LogError(ex, $"Artist with ID {songAddDTO.ArtistId} not found.");
                throw;
            }
            catch (NoSuchAlbumExistException ex)
            {
                _logger.LogError(ex, $"Album with ID {songAddDTO.AlbumId} not found.");
                throw;
            }
            catch (InvalidSongDuration ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            catch (InvalidGenreException ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            catch (UnableToAddSongException ex)
            {
                _logger.LogError($"Unable to add the song. {ex}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new song.");
                throw;
            }
        }


        public async Task<bool> DeleteRangeSongs(IList<int> ids)
        {
            try
            {
                await _songRepository.DeleteRange(ids);
                return true;
            }
            catch (NoSongsExistsException ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Unable to delete songs" + ex.Message);
                throw new NoSuchSongExistException("Unable to delete songs. " + ex.Message);
            }
        }

        [ExcludeFromCodeCoverage]
        public async Task<SongReturnDTO> UpdateSong(int songId, SongUpdateDTO songUpdateDTO)
        {
            try
            {
                var song = await _songRepository.GetById(songId);

                await _artistRepository.GetById(songUpdateDTO.ArtistId);

                if(songUpdateDTO.AlbumId != null)
                    await _albumRepository.GetById((int)songUpdateDTO.AlbumId);

                if (songUpdateDTO.Duration <= 0)
                {
                    throw new InvalidSongDuration();
                }

                if (!(Enum.TryParse<GenreType>(songUpdateDTO.Genre, true, out var genreTypeEnum)))
                {
                    throw new InvalidGenreException();
                }

                song.Title = songUpdateDTO.Title;
                song.Genre = genreTypeEnum;
                song.ArtistId = songUpdateDTO.ArtistId;
                song.AlbumId = songUpdateDTO.AlbumId;
                song.Url = songUpdateDTO.Url;
                song.Duration = songUpdateDTO.Duration;
                song.ImageUrl = songUpdateDTO.ImageUrl;

                var updatedSong = await _songRepository.Update(song);
                return _mapper.Map<SongReturnDTO>(updatedSong);
            }
            catch (NoSuchSongExistException ex)
            {
                _logger.LogError(ex, $"Song not found. {ex}");
                throw;
            }
            catch (NoSuchArtistExistException ex)
            {
                _logger.LogError(ex, $"Artist with ID {songUpdateDTO.ArtistId} not found.");
                throw;
            }
            catch (InvalidSongDuration ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            catch (InvalidGenreException ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            catch (NoSuchAlbumExistException ex)
            {
                _logger.LogError(ex, $"Album with ID {songUpdateDTO.AlbumId} not found.");
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
            catch (UnableToDeleteSongException ex)
            {
                _logger.LogError(ex, $"Unable to delete {ex}");
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
                await _artistRepository.GetById(artistId);
                var songs = (await _songRepository.GetSongsByArtistId(artistId)).ToList();
                if (songs.Count == 0)
                    throw new NoSongsExistsException();
                return _mapper.Map<IEnumerable<SongReturnDTO>>(songs);
            }
            catch (NoSuchArtistExistException ex)
            {
                _logger.LogError(ex, "Error retrieving album songs.");
                throw;
            }
            catch (NoSongsExistsException ex)
            {
                _logger.LogError(ex, "Error retrieving album songs.");
                throw;
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
                await _albumRepository.GetById(albumId);
                var songs = (await _songRepository.GetSongsByAlbumId(albumId)).ToList();
                if (songs.Count == 0)
                    throw new NoSongsExistsException();
                return _mapper.Map<IEnumerable<SongReturnDTO>>(songs);
            }
            catch(NoSuchAlbumExistException ex)
            {
                _logger.LogError(ex, "Error retrieving album songs.");
                throw;
            }
            catch (NoSongsExistsException ex)
            {
                _logger.LogError(ex, "Error retrieving album songs.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving songs by album ID {albumId}.");
                throw;
            }
        }

        [ExcludeFromCodeCoverage]
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
                if (!(Enum.TryParse<GenreType>(genre, true, out var genreTypeEnum)))
                {
                    throw new InvalidGenreException($"{genre} is not a valid genre"); 
                }
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
            catch (InvalidGenreException ex)
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
