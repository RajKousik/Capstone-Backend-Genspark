﻿using AutoMapper;
using MusicApplicationAPI.Exceptions.PlaylistExceptions;
using MusicApplicationAPI.Exceptions.UserExceptions;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Models.Enums;
using MusicApplicationAPI.Models.DTOs.PlaylistDTO;
using System.Data;

namespace MusicApplicationAPI.Services
{
    public class PlaylistService : IPlaylistService
    {
        #region Fields
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PlaylistService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IFavoriteRepository _favoriteRepository;
        #endregion

        #region Constructor
        public PlaylistService(IPlaylistRepository playlistRepository, IMapper mapper, ILogger<PlaylistService> logger, IUserRepository userRepository, IConfiguration configuration, IFavoriteRepository favoriteRepository)
        {
            _playlistRepository = playlistRepository;
            _mapper = mapper;
            _logger = logger;
            _userRepository = userRepository;
            _configuration = configuration;
            _favoriteRepository = favoriteRepository;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a new playlist.
        /// </summary>
        /// <param name="playlistCreateDTO">The playlist data to be added.</param>
        /// <returns>The added playlist data.</returns>
        /// <exception cref="MaximumPlaylistsReachedException">Thrown when a normal user has reached the maximum number of playlists.</exception>
        /// <exception cref="UserNotFoundException">Thrown when the user is not found.</exception>
        public async Task<PlaylistReturnDTO> AddPlaylist(PlaylistAddDTO playlistCreateDTO)
        {
            try
            {
                // Fetch the user to check their role and the number of playlists they have
                var user = await _userRepository.GetById(playlistCreateDTO.UserId);
                if (user == null)
                {
                    throw new NoSuchUserExistException($"User with ID {playlistCreateDTO.UserId} not found.");
                }

                // Check if the user has reached the maximum number of playlists if they are a normal user
                if (user.Role == RoleType.NormalUser && await HasReachedPlaylistLimit(user.UserId))
                {
                    throw new MaximumPlaylistsReachedException("Normal user has reached the maximum number of playlists.");
                }

                var playlist = _mapper.Map<Playlist>(playlistCreateDTO);
                var addedPlaylist = await _playlistRepository.Add(playlist);
                return _mapper.Map<PlaylistReturnDTO>(addedPlaylist);
            }
            catch (NoSuchUserExistException ex)
            {
                _logger.LogError(ex, "User not found.");
                throw;
            }
            catch (MaximumPlaylistsReachedException ex)
            {
                _logger.LogError(ex, "Maximum number of playlists reached for user.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding playlist.");
                throw;
            }
        }

        /// <summary>
        /// Updates an existing playlist.
        /// </summary>
        /// <param name="playlistId">The ID of the playlist to be updated.</param>
        /// <param name="playlistUpdateDTO">The updated playlist data.</param>
        /// <returns>The updated playlist data.</returns>
        public async Task<PlaylistReturnDTO> UpdatePlaylist(int playlistId, PlaylistUpdateDTO playlistUpdateDTO)
        {
            try
            {
                var playlist = await _playlistRepository.GetById(playlistId);
                _mapper.Map(playlistUpdateDTO, playlist);
                var updatedPlaylist = await _playlistRepository.Update(playlist);
                return _mapper.Map<PlaylistReturnDTO>(updatedPlaylist);
            }
            catch (NoSuchPlaylistExistException ex)
            {
                _logger.LogError(ex, "Playlist not found.");
                throw;
            }
            catch (UnableToUpdatePlaylistException ex)
            {
                _logger.LogError(ex, "Error updating playlist.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating playlist.");
                throw;
            }
        }

        /// <summary>
        /// Retrieves a playlist by its ID.
        /// </summary>
        /// <param name="playlistId">The ID of the playlist to be retrieved.</param>
        /// <returns>The playlist data.</returns>
        public async Task<PlaylistReturnDTO> GetPlaylistById(int playlistId)
        {
            try
            {
                var playlist = await _playlistRepository.GetById(playlistId);
                return _mapper.Map<PlaylistReturnDTO>(playlist);
            }
            catch (NoSuchPlaylistExistException ex)
            {
                _logger.LogError(ex, "Playlist not found.");
                throw;  
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving playlist by ID.");
                throw;
            }
        }

        /// <summary>
        /// Retrieves all playlists.
        /// </summary>
        /// <returns>A list of all playlists.</returns>
        public async Task<IEnumerable<PlaylistReturnDTO>> GetAllPlaylists()
        {
            try
            {
                var playlists = (await _playlistRepository.GetAll()).ToList();
                if(playlists.Count == 0)
                {
                    throw new NoPlaylistsExistsException();
                }
                return _mapper.Map<IEnumerable<PlaylistReturnDTO>>(playlists);
            }
            catch (NoPlaylistsExistsException ex)
            {
                _logger.LogError(ex, "Error retrieving all playlists.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all playlists.");
                throw;
            }
        }

        /// <summary>
        /// Deletes a playlist by its ID.
        /// </summary>
        /// <param name="playlistId">The ID of the playlist to be deleted.</param>
        /// <returns>The deleted playlist data.</returns>
        public async Task<PlaylistReturnDTO> DeletePlaylist(int playlistId)
        {
            try
            {
                await DeleteRelatedFavorites(playlistId);

                var deletedPlaylist = await _playlistRepository.Delete(playlistId);


                return _mapper.Map<PlaylistReturnDTO>(deletedPlaylist);
            }
            catch (NoSuchPlaylistExistException ex)
            {
                _logger.LogError(ex, "Playlist not found.");
                throw;
            }
            catch (UnableToDeletePlaylistException ex)
            {
                _logger.LogError(ex, "Error deleting playlist.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting playlist.");
                throw;
            }
        }


        /// <summary>
        /// Retrieves playlists by a specific user ID.
        /// </summary>
        /// <param name="userId">The user ID for which to retrieve playlists.</param>
        /// <returns>A list of playlists for the specified user.</returns>
        public async Task<IEnumerable<PlaylistReturnDTO>> GetPlaylistsByUserId(int userId)
        {
            try
            {
                // Check if the user exists
                var user = await _userRepository.GetById(userId);
                if (user == null)
                {
                    throw new NoSuchUserExistException($"User with ID {userId} does not exist.");
                }

                var playlists = (await _playlistRepository.GetAll()).ToList();

                if (playlists.Count == 0)
                {
                    throw new NoPlaylistsExistsException("No playlists exist in the database.");
                }

                var userPlaylists = playlists.Where(p => p.UserId == userId).ToList();

                if (userPlaylists.Count == 0)
                {
                    throw new NoPlaylistsExistsException($"No playlists found for user ID {userId}.");
                }

                return _mapper.Map<IEnumerable<PlaylistReturnDTO>>(userPlaylists);
            }
            catch (NoPlaylistsExistsException ex)
            {
                _logger.LogError(ex, "Error retrieving playlists.");
                throw;
            }
            catch (NoSuchUserExistException ex)
            {
                _logger.LogError(ex, "User not found.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving playlists for user ID: {userId}.");
                throw;
            }
        }


        /// <summary>
        /// Retrieves all public playlists.
        /// </summary>
        /// <returns>A list of public playlists.</returns>
        public async Task<IEnumerable<PlaylistReturnDTO>> GetPublicPlaylists()
        {
            try
            {
                var playlists = (await _playlistRepository.GetAll()).ToList(); 
                if (playlists.Count == 0) {
                    throw new NoPlaylistsExistsException();
                }
                var publicPlaylists = (playlists.Where(p => p.IsPublic)).ToList();
                if (publicPlaylists.Count == 0) { 
                    throw new NoPlaylistsExistsException();
                }
                return _mapper.Map<IEnumerable<PlaylistReturnDTO>>(publicPlaylists);
            }
            catch (NoPlaylistsExistsException ex)
            {
                _logger.LogError(ex, "Error retrieving all playlists.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving public playlists.");
                throw;
            }
        }

        #endregion


        #region Private Methods
        /// <summary>
        /// Checks if a user has reached the maximum number of playlists for normal users.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>True if the user has reached the maximum limit; otherwise, false.</returns>
        private async Task<bool> HasReachedPlaylistLimit(int userId)
        {

            int maxPlaylists = _configuration.GetValue<int>("PlaylistSettings:MaxPlaylistsPerNormalUser");
            var userPlaylists = await _playlistRepository.GetPlaylistsByUserId(userId);
            return userPlaylists.Count() >= maxPlaylists;
        }


        private async Task DeleteRelatedFavorites(int playlistId)
        {
            try
            {
                // Retrieve all favorites related to the playlist
                var favorites = (await _favoriteRepository.GetAll()).Where(f => f.PlaylistId == playlistId);

                // Delete each favorite
                foreach (var favorite in favorites)
                {
                    await _favoriteRepository.Delete(favorite.FavoriteId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting favorites for playlist ID {playlistId}.");
                throw;
            }
        }
        #endregion
    }
}
