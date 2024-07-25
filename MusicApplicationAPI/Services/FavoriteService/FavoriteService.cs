using AutoMapper;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Exceptions.FavoriteExceptions;
using MusicApplicationAPI.Exceptions.SongExceptions;
using MusicApplicationAPI.Exceptions.UserExceptions;
using MusicApplicationAPI.Models.DTOs.PlaylistDTO;
using MusicApplicationAPI.Models.DTOs.SongDTO;
using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Models.DTOs.FavoriteDTO;
using MusicApplicationAPI.Exceptions.PlaylistExceptions;

namespace MusicApplicationAPI.Services.FavoriteService
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly ISongRepository _songRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<FavoriteService> _logger;

        public FavoriteService(
            IFavoriteRepository favoriteRepository,
            IPlaylistRepository playlistRepository,
            ISongRepository songRepository,
            IUserRepository userRepository,
            IMapper mapper,
            ILogger<FavoriteService> logger)
        {
            _favoriteRepository = favoriteRepository;
            _playlistRepository = playlistRepository;
            _songRepository = songRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Marks a song as a favorite for a user.
        /// </summary>
        /// <param name="favoriteDTO">The favorite data transfer object containing user and song information.</param>
        /// <exception cref="NoSuchUserExistException">Thrown when the user does not exist.</exception>
        /// <exception cref="NoSuchSongExistException">Thrown when the song does not exist.</exception>
        /// <exception cref="AlreadyMarkedAsFavorite">Thrown when the song is already marked as favorite by the user.</exception>
        /// <exception cref="UnableToAddFavoriteException">Thrown when unable to mark the song as favorite due to an unknown error.</exception>
        public async Task MarkSongAsFavorite(FavoriteSongDTO favoriteSongDTO)
        {
            try
            {
                var user = await _userRepository.GetById(favoriteSongDTO.UserId);
                if (user == null)
                    throw new NoSuchUserExistException("User not found.");


                var song = await _songRepository.GetById(favoriteSongDTO.SongId);
                if (song == null)
                    throw new NoSuchSongExistException("Song not found.");

                var isAlreadyLiked = (await _favoriteRepository.GetAll())
                    .Any(fv => fv.UserId == favoriteSongDTO.UserId && fv.SongId == favoriteSongDTO.SongId);

                if (isAlreadyLiked)
                    throw new AlreadyMarkedAsFavorite("This song has already been marked as favorite.");
                Favorite favorite = new Favorite()
                {
                    UserId = favoriteSongDTO.UserId,
                    SongId = favoriteSongDTO.SongId,
                    PlaylistId = null
                };
                await _favoriteRepository.Add(favorite);
            }
            catch (NoSuchUserExistException ex)
            {
                _logger.LogError(ex, "User not found.");
                throw;
            }
            catch (NoSuchSongExistException ex)
            {
                _logger.LogError(ex, "Song not found.");
                throw;
            }
            catch (AlreadyMarkedAsFavorite ex)
            {
                _logger.LogError(ex, "Song already marked as favorite.");
                throw;
            }
            catch (UnableToAddFavoriteException ex)
            {
                _logger.LogError(ex, "Unable to mark song as favorite.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking song as favorite.");
                throw;
            }
        }

        /// <summary>
        /// Removes a song from a user's favorites.
        /// </summary>
        /// <param name="favoriteDTO">The favorite data transfer object containing user and song information.</param>
        /// <exception cref="NoSuchUserExistException">Thrown when the user does not exist.</exception>
        /// <exception cref="NoSuchSongExistException">Thrown when the song does not exist.</exception>
        /// <exception cref="NotMarkedAsFavorite">Thrown when the song is not marked as favorite by the user.</exception>
        /// <exception cref="UnableToDeleteFavoriteException">Thrown when unable to remove the song from favorites due to an unknown error.</exception>
        public async Task RemoveSongFromFavorites(FavoriteSongDTO favoriteSongDTO)
        {
            try
            {
                var user = await _userRepository.GetById(favoriteSongDTO.UserId);
                if (user == null)
                    throw new NoSuchUserExistException("User not found.");



                var song = await _songRepository.GetById(favoriteSongDTO.SongId);
                if (song == null)
                    throw new NoSuchSongExistException("Song not found.");

                var isLiked = (await _favoriteRepository.GetAll())
                    .FirstOrDefault(fv => fv.UserId == favoriteSongDTO.UserId && fv.SongId == favoriteSongDTO.SongId);

                if (isLiked == null)
                    throw new NotMarkedAsFavorite("This song has not been marked as favorite.");

                await _favoriteRepository.Delete(isLiked.FavoriteId);
            }
            catch (NoSuchUserExistException ex)
            {
                _logger.LogError(ex, "User not found.");
                throw;
            }
            catch (NoSuchSongExistException ex)
            {
                _logger.LogError(ex, "Song not found.");
                throw;
            }
            catch (NotMarkedAsFavorite ex)
            {
                _logger.LogError(ex, "Song not marked as favorite.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing song from favorites.");
                throw new UnableToDeleteFavoriteException("Unable to remove song from favorites.");
            }
        }

        /// <summary>
        /// Marks a playlist as a favorite for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="playlistId">The ID of the playlist.</param>
        /// <exception cref="NoSuchUserExistException">Thrown when the user does not exist.</exception>
        /// <exception cref="NoSuchPlaylistExistException">Thrown when the playlist does not exist.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the playlist is private and does not belong to the user.</exception>
        /// <exception cref="UnableToAddFavoriteException">Thrown when unable to mark the playlist as favorite due to an unknown error.</exception>
        public async Task MarkPlaylistAsFavorite(FavoritePlaylistDTO favoritePlaylistDTO)
        {
            try
            {
                var user = await _userRepository.GetById(favoritePlaylistDTO.UserId);
                if (user == null) throw new NoSuchUserExistException("User not found.");

                var playlist = await _playlistRepository.GetById(favoritePlaylistDTO.PlaylistId);
                if (playlist == null) throw new NoSuchPlaylistExistException("Playlist not found.");

                if (playlist.UserId != favoritePlaylistDTO.UserId && !playlist.IsPublic)
                    throw new InvalidOperationException("Cannot favorite a private playlist that does not belong to the user.");

                var isAlreadyFavorited = (await _favoriteRepository.GetAll())
                    .Any(fv => fv.UserId == favoritePlaylistDTO.UserId && fv.PlaylistId == favoritePlaylistDTO.PlaylistId);

                if (isAlreadyFavorited)
                    throw new AlreadyMarkedAsFavorite("This playlist has already been marked as favorite.");

                Favorite favorite = new Favorite()
                {
                    UserId = favoritePlaylistDTO.UserId,
                    SongId = null,
                    PlaylistId = favoritePlaylistDTO.PlaylistId
                };

                await _favoriteRepository.Add(favorite);
            }
            catch (NoSuchUserExistException ex)
            {
                _logger.LogError(ex, "User not found.");
                throw;
            }
            catch (NoSuchPlaylistExistException ex)
            {
                _logger.LogError(ex, "Playlist not found.");
                throw;
            }
            catch (AlreadyMarkedAsFavorite ex)
            {
                _logger.LogError(ex, "Playlist already marked as favorite.");
                throw;
            }
            catch (UnableToAddFavoriteException ex)
            {
                _logger.LogError(ex, "Unable to mark playlist as favorite.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking playlist as favorite.");
                throw;
            }
        }

        /// <summary>
        /// Removes a playlist from a user's favorites.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="playlistId">The ID of the playlist.</param>
        /// <exception cref="NoSuchUserExistException">Thrown when the user does not exist.</exception>
        /// <exception cref="NoSuchPlaylistExistException">Thrown when the playlist does not exist.</exception>
        /// <exception cref="UnableToDeleteFavoriteException">Thrown when unable to remove the playlist from favorites due to an unknown error.</exception>
        public async Task RemovePlaylistFromFavorites(int userId, int playlistId)
        {
            try
            {
                var user = await _userRepository.GetById(userId);
                if (user == null) throw new NoSuchUserExistException("User not found.");

                var playlist = await _playlistRepository.GetById(playlistId);
                if (playlist == null) throw new NoSuchPlaylistExistException("Playlist not found.");

                var isFavorited = (await _favoriteRepository.GetAll())
                    .FirstOrDefault(fv => fv.UserId == userId && fv.PlaylistId == playlistId);

                if (isFavorited == null)
                    throw new NotMarkedAsFavorite("This playlist has not been marked as favorite.");

                await _favoriteRepository.Delete(isFavorited.FavoriteId);
            }
            catch (NoSuchUserExistException ex)
            {
                _logger.LogError(ex, "User not found.");
                throw;
            }
            catch (NoSuchPlaylistExistException ex)
            {
                _logger.LogError(ex, "Playlist not found.");
                throw;
            }
            catch (NotMarkedAsFavorite ex)
            {
                _logger.LogError(ex, "Playlist not marked as favorite.");
                throw;
            }
            catch (UnableToDeleteFavoriteException ex)
            {
                _logger.LogError(ex, "Unable to remove playlist from favorites.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing playlist from favorites.");
                throw;
            }
        }

        /// <summary>
        /// Retrieves a user's favorite songs.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of favorite songs for the specified user.</returns>
        /// <exception cref="NoSuchUserExistException">Thrown when the user does not exist.</exception>
        /// <exception cref="NoFavoritesExistException">Thrown when no favorite songs are found.</exception>
        public async Task<IEnumerable<SongReturnDTO>> GetFavoriteSongsByUserId(int userId)
        {
            try
            {
                // Check if the user exists
                var user = await _userRepository.GetById(userId);
                if (user == null)
                    throw new NoSuchUserExistException("User not found.");

                // Retrieve all favorites and filter for the specific user's favorites
                var allFavorites = await _favoriteRepository.GetAll();
                var userFavorites = allFavorites
                    .Where(f => f.UserId == userId && f.SongId != null)
                    .Select(f => f.SongId.Value)
                    .ToList();

                // Fetch all songs and filter based on favorite IDs
                var allSongs = await _songRepository.GetAll();
                var favoriteSongs = allSongs
                    .Where(s => userFavorites.Contains(s.SongId))
                    .ToList();

                // If no favorite songs are found, throw an exception
                if (!favoriteSongs.Any())
                    throw new NoFavoritesExistsException("No favorite songs found.");

                // Map to DTO and return
                return _mapper.Map<IEnumerable<SongReturnDTO>>(favoriteSongs);
            }
            catch (NoSuchUserExistException ex)
            {
                _logger.LogError(ex, "User not found.");
                throw;
            }
            catch (NoFavoritesExistsException ex)
            {
                _logger.LogError(ex, "No favorite songs found.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving favorite songs.");
                throw new UnableToRetrieveFavoritesException("Unable to retrieve favorite songs.");
            }
        }


        /// <summary>
        /// Retrieves a user's favorite playlists.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of favorite playlists for the specified user.</returns>
        /// <exception cref="NoSuchUserExistException">Thrown when the user does not exist.</exception>
        /// <exception cref="NoFavoritesExistException">Thrown when no favorite playlists are found.</exception>
        public async Task<IEnumerable<PlaylistReturnDTO>> GetFavoritePlaylistsByUserId(int userId)
        {
            try
            {
                // Check if the user exists
                var user = await _userRepository.GetById(userId);
                if (user == null)
                    throw new NoSuchUserExistException("User not found.");

                // Retrieve all favorites and filter for the specific user's playlist favorites
                var allFavorites = await _favoriteRepository.GetAll();
                var userFavoritePlaylists = allFavorites
                    .Where(f => f.UserId == userId && f.PlaylistId != null)
                    .Select(f => f.PlaylistId.Value)
                    .ToList();

                // Fetch all playlists and filter based on favorite IDs
                var allPlaylists = await _playlistRepository.GetAll();
                var favoritePlaylists = allPlaylists
                    .Where(p => userFavoritePlaylists.Contains(p.PlaylistId))
                    .ToList();

                // If no favorite playlists are found, throw an exception
                if (!favoritePlaylists.Any())
                    throw new NoFavoritesExistsException("No favorite playlists found.");

                // Map to DTO and return
                return _mapper.Map<IEnumerable<PlaylistReturnDTO>>(favoritePlaylists);
            }
            catch (NoSuchUserExistException ex)
            {
                _logger.LogError(ex, "User not found.");
                throw;
            }
            catch (NoFavoritesExistsException ex)
            {
                _logger.LogError(ex, "No favorite playlists found.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving favorite playlists.");
                throw new UnableToRetrieveFavoritesException("Unable to retrieve favorite playlists.");
            }
        }

    }
}
