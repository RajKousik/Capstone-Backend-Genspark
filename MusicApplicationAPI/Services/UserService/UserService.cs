﻿using AutoMapper;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Exceptions.UserExceptions;
using MusicApplicationAPI.Models.DTOs.UserDTO;

namespace MusicApplicationAPI.Services.UserService
{
    /// <summary>
    /// Service class to handle user-related operations.
    /// </summary>
    public class UserService : IUserService
    {
        #region Fields
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        #endregion

        #region Constructor
        public UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the user profile.
        /// </summary>
        /// <param name="userUpdateDTO">The updated user data.</param>
        /// <returns>The updated user data.</returns>
        public async Task<UserReturnDTO> UpdateUserProfile(UserUpdateDTO userUpdateDTO, int userId)
        {
            try
            {
                var user = await _userRepository.GetById(userId);
                user.DOB = userUpdateDTO.DOB.ToDateTime(TimeOnly.MinValue);
                user.Username = userUpdateDTO.Username;
                var updatedUser = await _userRepository.Update(user);
                return _mapper.Map<UserReturnDTO>(updatedUser);
            }
            catch (NoSuchUserExistException ex)
            {
                _logger.LogError($"User not found. {ex}");
                throw;
            }
            catch (UnableToUpdateUserException ex)
            {
                _logger.LogError($"Unable to update user, {ex}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating user profile {ex}");
                throw;
            }
        }

        /// <summary>
        /// Gets a user by ID.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>The user data.</returns>
        public async Task<UserReturnDTO> GetUserById(int userId)
        {
            try
            {
                var user = await _userRepository.GetById(userId);
                return _mapper.Map<UserReturnDTO>(user);
            }
            catch (NoSuchUserExistException ex)
            {
                _logger.LogError(ex, "User not found.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user by ID.");
                throw;
            }
        }

        /// <summary>
        /// Gets a user by email.
        /// </summary>
        /// <param name="email">The user email.</param>
        /// <returns>The user data.</returns>
        public async Task<UserReturnDTO> GetUserByEmail(string email)
        {
            try
            {
                var user = await _userRepository.GetUserByEmail(email);
                return _mapper.Map<UserReturnDTO>(user);
            }
            catch (NoSuchUserExistException ex)
            {
                _logger.LogError(ex, "User not found.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user by email.");
                throw;
            }
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>A list of users.</returns>
        public async Task<IEnumerable<UserReturnDTO>> GetAllUsers()
        {
            try
            {
                var users = (await _userRepository.GetAll()).ToList();
                if (users.Count == 0)
                    throw new NoUsersExistsExistsException("No users in the database");
                return _mapper.Map<IEnumerable<UserReturnDTO>>(users);
            }
            catch (NoUsersExistsExistsException ex)
            {
                _logger.LogError(ex, "Error retrieving all users.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all users.");
                throw;
            }
        }

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>The deleted user data.</returns>
        public async Task<UserReturnDTO> DeleteUser(int userId)
        {
            try
            {
                var user = await _userRepository.Delete(userId);
                return _mapper.Map<UserReturnDTO>(user);
            }
            catch (NoSuchUserExistException ex)
            {
                _logger.LogError(ex, "User not found.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user.");
                throw;
            }
        }

        #endregion

    }
}
