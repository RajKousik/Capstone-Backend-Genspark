using AutoMapper;
using MusicApplicationAPI.Exceptions.PlaylistExceptions;
using MusicApplicationAPI.Exceptions.PlaylistSongExceptions;
using MusicApplicationAPI.Exceptions.SongExceptions;
using MusicApplicationAPI.Exceptions.UserExceptions;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Models.DTOs.PlaylistSongDTO;
using MusicApplicationAPI.Models.DTOs.SongDTO;
using MusicApplicationAPI.Models.Enums;
using System.Data;

namespace MusicApplicationAPI.Services
{
    public class PlaylistSongService : IPlaylistSongService
    {
        #region Private Fields
        private readonly IPlaylistRepository _playlistRepository;
        private readonly ISongRepository _songRepository;
        private readonly IPlaylistSongRepository _playlistSongRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PlaylistSongService> _logger;
        private readonly IConfiguration _configuration;

        #endregion

        #region Constructor
        public PlaylistSongService(
            IPlaylistRepository playlistRepository,
            ISongRepository songRepository,
            IPlaylistSongRepository playlistSongRepository,
            IMapper mapper,
            ILogger<PlaylistSongService> logger,
            IConfiguration configuration,
            IUserRepository userRepository)
        {
            _playlistRepository = playlistRepository;
            _songRepository = songRepository;
            _playlistSongRepository = playlistSongRepository;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
            _userRepository = userRepository;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Adds a song to a playlist.
        /// </summary>
        /// <param name="playlistSongDTO">The playlist song DTO.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="NoSuchPlaylistExistException">Thrown when the specified playlist does not exist.</exception>
        /// <exception cref="NoSuchSongExistException">Thrown when the specified song does not exist.</exception>
        /// <exception cref="UnableToAddPlaylistSongException">Thrown when the song could not be added to the playlist.</exception>
        public async Task<PlaylistSongReturnDTO> AddSongToPlaylist(PlaylistSongDTO playlistSongDTO)
        {
            try
            {
                var playlist = await _playlistRepository.GetById(playlistSongDTO.PlaylistId);
                if (playlist == null)
                    throw new NoSuchPlaylistExistException("Playlist not found.");

                var song = await _songRepository.GetById(playlistSongDTO.SongId);
                if (song == null)
                    throw new NoSuchSongExistException("Song not found.");

                // Check if the song is already in the playlist
                var playlistSongs = await _playlistSongRepository.GetPlaylistSongsByPlaylistId(playlistSongDTO.PlaylistId);
                if (playlistSongs.Any(ps => ps.SongId == playlistSongDTO.SongId))
                    throw new UnableToAddPlaylistSongException("The song is already in the playlist.");


                if (await HasReachedSongLimitForPlaylist(playlist.PlaylistId, playlist.UserId))
                    throw new UnableToAddPlaylistSongException("The playlist has reached the maximum number of songs.");

                var playlistSong = _mapper.Map<PlaylistSong>(playlistSongDTO);
                var addedPlaylistSong = await _playlistSongRepository.Add(playlistSong);
                return _mapper.Map<PlaylistSongReturnDTO>(addedPlaylistSong);
            }
            catch (NoSuchPlaylistExistException ex)
            {
                _logger.LogError(ex, "Playlist not found.");
                throw;
            }
            catch (NoSuchSongExistException ex)
            {
                _logger.LogError(ex, "Song not found.");
                throw;
            }
            catch (UnableToAddPlaylistSongException ex)
            {
                _logger.LogError(ex, "Unable to add song to playlist.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding song to playlist.");
                throw;
            }
        }

        /// <summary>
        /// Removes a song from a playlist.
        /// </summary>
        /// <param name="playlistSongDTO">The playlist song DTO containing playlistId and songId.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="NoSuchPlaylistException">Thrown when the specified playlist does not exist.</exception>
        /// <exception cref="NoSuchSongExistException">Thrown when the specified song does not exist in the playlist.</exception>
        /// <exception cref="UnableToDeletePlaylistSongException">Thrown when the song could not be removed from the playlist.</exception>
        public async Task<PlaylistSongReturnDTO> RemoveSongFromPlaylist(PlaylistSongDTO playlistSongDTO)
        {
            try
            {
                // Check if the playlist exists
                var playlist = await _playlistRepository.GetById(playlistSongDTO.PlaylistId);

                // Check if the song exists
                var song = await _songRepository.GetById(playlistSongDTO.SongId);

                // Check if the playlist contains the song
                var playlistSong = (await _playlistSongRepository.GetAll()).FirstOrDefault(ps => (ps.SongId == playlistSongDTO.SongId) && (ps.PlaylistId == playlistSongDTO.PlaylistId));

                if (playlistSong == null)
                    throw new NoSuchPlaylistSongExistException("Song not found in the playlist.");

                var removedPlayListSong = await _playlistSongRepository.Delete(playlistSong.PlaylistSongId);

                return _mapper.Map<PlaylistSongReturnDTO>(removedPlayListSong);
            }
            catch (NoSuchPlaylistExistException ex)
            {
                _logger.LogError(ex, "Playlist not found.");
                throw;
            }
            catch (NoSuchSongExistException ex)
            {
                _logger.LogError(ex, "Song not found.");
                throw;
            }
            catch (NoSuchPlaylistSongExistException ex)
            {
                _logger.LogError(ex, "Song not found in the playlist.");
                throw;
            }
            catch (UnableToDeletePlaylistSongException ex)
            {
                _logger.LogError(ex, "Unable to delete.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing song from playlist.");
                throw;
            }
        }


        /// <summary>
        /// Retrieves all songs in a specific playlist.
        /// </summary>
        /// <param name="playlistId">The ID of the playlist.</param>
        /// <returns>A list of songs in the specified playlist.</returns>
        /// <exception cref="NoSuchPlaylistException">Thrown when the specified playlist does not exist.</exception>
        /// <exception cref="NoSongsInPlaylistException">Thrown when no songs are found in the playlist.</exception>
        public async Task<IEnumerable<SongReturnDTO>> GetSongsInPlaylist(int playlistId)
        {
            try
            {
                var playlist = await _playlistRepository.GetById(playlistId);
                if (playlist == null)
                    throw new NoSuchPlaylistExistException("Playlist not found.");

                var playlistSongs = (await _playlistSongRepository.GetPlaylistSongsByPlaylistId(playlistId)).ToList();
                if (playlistSongs.Count == 0)
                    throw new NoSongsInPlaylistException("No songs found in playlist.");

                // Get the IDs of the songs in the playlist
                var songIds = playlistSongs.Select(ps => ps.SongId).ToList();

                // Retrieve songs with the specified IDs
                var songs = (await _songRepository.GetAll())
                    .Where(s => songIds.Contains(s.SongId));

                return _mapper.Map<IEnumerable<SongReturnDTO>>(songs);
            }
            catch (NoSuchPlaylistExistException ex)
            {
                _logger.LogError(ex, "Playlist not found.");
                throw;
            }
            catch (NoSongsInPlaylistException ex)
            {
                _logger.LogError(ex, "No songs found in playlist.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving songs in playlist.");
                throw;
            }
        }

        /// <summary>
        /// Clears all songs from a playlist.
        /// </summary>
        /// <param name="playlistId">The ID of the playlist to clear.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="NoSuchPlaylistException">Thrown when the specified playlist does not exist.</exception>
        /// <exception cref="NoSongsInPlaylistException">Thrown when no songs are found in the playlist to clear.</exception>
        /// <exception cref="UnableToClearPlaylistException">Thrown when the playlist could not be cleared.</exception>
        public async Task ClearPlaylist(int playlistId)
        {
            try
            {
                // Check if the playlist exists
                var playlist = await _playlistRepository.GetById(playlistId);
                if (playlist == null)
                    throw new NoSuchPlaylistExistException("Playlist not found.");

                // Get all songs in the playlist
                var playlistSongs = (await _playlistSongRepository.GetPlaylistSongsByPlaylistId(playlistId)).ToList();
                if (playlistSongs.Count == 0)
                    throw new NoSongsInPlaylistException("No songs found in playlist to clear.");

                // Remove each song from the playlist
                foreach (var playlistSong in playlistSongs)
                {
                    await _playlistSongRepository.Delete(playlistSong.PlaylistSongId);
                }
            }
            catch (NoSuchPlaylistExistException ex)
            {
                _logger.LogError(ex, "Playlist not found.");
                throw;
            }
            catch (NoSongsInPlaylistException ex)
            {
                _logger.LogError(ex, "No songs found in playlist to clear.");
                throw;
            }
            catch (UnableToClearPlaylistException ex)
            {
                _logger.LogError(ex, "No songs found in playlist to clear.");
                throw;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing playlist.");
                throw;
            }
        }


        /// <summary>
        /// Gets the count of songs in a playlist.
        /// </summary>
        /// <param name="playlistId">The ID of the playlist.</param>
        /// <returns>The count of songs in the specified playlist.</returns>
        /// <exception cref="NoSuchPlaylistException">Thrown when the specified playlist does not exist.</exception>
        public async Task<int> GetSongCountInPlaylist(int playlistId)
        {
            try
            {
                var playlist = await _playlistRepository.GetById(playlistId);
                if (playlist == null)
                    throw new NoSuchPlaylistExistException("Playlist not found.");

                return (await _playlistSongRepository.GetPlaylistSongsByPlaylistId(playlistId)).ToList().Count();
            }
            catch (NoSuchPlaylistExistException ex)
            {
                _logger.LogError(ex, "Playlist not found.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting songs in playlist.");
                throw;
            }
        }

        #endregion

        #region Private Methods
        private async Task<bool> HasReachedSongLimitForPlaylist(int playlistId, int userId)
        {
            // Fetch user to check the role
            var user = await _userRepository.GetById(userId);
            if (user == null)
            {
                _logger.LogError($"User with ID {userId} not found.");
                throw new NoSuchUserExistException("User not found.");
            }

            // Check if the user is a normal user
            if (user.Role == RoleType.NormalUser)
            {
                int maxSongs = _configuration.GetValue<int>("PlaylistSettings:MaxSongsPerPlaylistForNormalUser");
                var playlistSongs = await _playlistSongRepository.GetPlaylistSongsByPlaylistId(playlistId);

                if (playlistSongs.Count() >= maxSongs)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }

}

