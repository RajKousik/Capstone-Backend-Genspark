using AutoMapper;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Models.Enums;
using MusicApplicationAPI.Exceptions.UserExceptions;
using MusicApplicationAPI.Models.DTOs.UserDTO;
using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Models.DTOs.OtherDTO;
using MusicApplicationAPI.Exceptions.ArtistExceptions;
using MusicApplicationAPI.Models.DTOs.ArtistDTO;
using MusicApplicationAPI.Repositories;

namespace MusicApplicationAPI.Services.UserService
{
    /// <summary>
    /// Service class to handle user-related operations.
    /// </summary>
    public class UserService : IUserService
    {
        #region Fields
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender _emailSenderService;
        private readonly IPremiumUserRepository _premiumUserRepository;
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        #endregion

        #region Constructor
        public UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger, IPasswordService passwordService, IPremiumUserRepository premiumUserRepository, IEmailSender emailSenderService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
            _passwordService = passwordService;
            _premiumUserRepository = premiumUserRepository;
            _emailSenderService = emailSenderService;
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
                var result = _mapper.Map<UserReturnDTO>(user);
                return result;
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


        public async Task<UserReturnDTO> GetAdminById(int adminId)
        {
            try
            {
                var user = await _userRepository.GetById(adminId);
                if(user.Role != RoleType.Admin)
                {
                    throw new NoSuchUserExistException();
                }
                var result = _mapper.Map<UserReturnDTO>(user);
                return result;
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
                if(user == null)
                {
                    throw new NoSuchUserExistException();
                }
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


        public async Task<UserRegisterReturnDTO> AddAdmin(UserRegisterDTO adminRegisterDTO)
        {
            User user;
            try
            {
                var emailExists = await _userRepository.GetUserByEmail(adminRegisterDTO.Email);
                if (emailExists != null)
                {
                    throw new DuplicateEmailException("Email id is already registered");
                }

                DateTime dateOfBirth = adminRegisterDTO.DOB.ToDateTime(TimeOnly.MinValue);


                user = new User()
                {
                    Username = adminRegisterDTO.Username,
                    Email = adminRegisterDTO.Email,
                    DOB = dateOfBirth,
                    Role = RoleType.Admin,
                    Status = "Active",
                };

                user.PasswordHash = _passwordService.HashPassword(adminRegisterDTO.Password, out byte[] key);
                user.PasswordHashKey = key;

                var addedUser = await _userRepository.Add(user);

                UserRegisterReturnDTO userRegisterReturnDTO= _mapper.Map<UserRegisterReturnDTO>(addedUser);

                return userRegisterReturnDTO;
            }
            catch (UnableToAddUserException ex)
            {
                _logger.LogError(ex.Message);
                throw new UnableToAddUserException(ex.Message);
            }
            catch (DuplicateEmailException ex)
            {
                _logger.LogError(ex.Message);
                throw new DuplicateEmailException(ex.Message);
            }
            catch (InvalidPasswordException ex)
            {
                _logger.LogError(ex.Message);
                throw new InvalidPasswordException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                var message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>A list of users (non-admins).</returns>
        public async Task<IEnumerable<UserReturnDTO>> GetAllUsers()
        {
            try
            {
                var users = (await _userRepository.GetAll()).Where(u => u.Role != RoleType.Admin).ToList();
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
        /// Gets all users.
        /// </summary>
        /// <returns>A list of premium users </returns>
        public async Task<IEnumerable<PremiumUser>> GetAllPremiumUsers()
        {
            try
            {
                //var users = (await _userRepository.GetAll()).Where(u => u.Role == RoleType.PremiumUser).ToList();

                var premiumUsers = (await _premiumUserRepository.GetAll()).ToList();
                if (premiumUsers.Count == 0)
                    throw new NoUsersExistsExistsException("No users in the database");
                return _mapper.Map<IEnumerable<PremiumUser>>(premiumUsers);
            }
            catch (NoUsersExistsExistsException ex)
            {
                _logger.LogError(ex, "Error retrieving all Premium users.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all Premium users.");
                throw;
            }
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>A list of admin users </returns>
        public async Task<IEnumerable<UserReturnDTO>> GetAllAdminUsers()
        {
            try
            {
                var users = (await _userRepository.GetAll()).Where(u => u.Role == RoleType.Admin).ToList();
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
            catch(UnableToDeleteUserException ex)
            {
                _logger.LogError(ex, "Unable to Delete");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user.");
                throw;
            }
        }


        public async Task<bool> ChangePassword(ChangePasswordRequestDTO requestDTO, int userId)
        {
            try
            {
                var user = await _userRepository.GetById(userId);

                if (user == null)
                    throw new NoSuchUserExistException("User not found.");

                if (!_passwordService.VerifyPassword(requestDTO.CurrentPassword, user.PasswordHash, user.PasswordHashKey))
                    return false;

                user.PasswordHash = _passwordService.HashPassword(requestDTO.NewPassword, out byte[] key);
                user.PasswordHashKey = key;
                await _userRepository.Update(user);
                return true;
            }
            catch (NoSuchUserExistException ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            catch (InvalidPasswordException ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            catch (UnableToUpdateUserException ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }


        public async Task<PremiumUser> UpgradeUserToPremium(int userId, PremiumRequestDTO premiumRequest)
        {
            try
            {
                var user = await _userRepository.GetById(userId);

                if (user == null)
                {
                    throw new NoSuchUserExistException($"User with ID {userId} does not exist.");
                }

                user.Role = RoleType.PremiumUser;
                await _userRepository.Update(user);

                var existingPremiumUser = await _premiumUserRepository.GetByUserId(userId);
                DateTime newEndDate;

                if (existingPremiumUser == null)
                {
                    // New Premium Subscription
                    newEndDate = DateTime.UtcNow.AddDays(premiumRequest.DurationInDays);
                    var premiumUser = new PremiumUser
                    {
                        UserId = userId,
                        StartDate = DateTime.UtcNow,
                        EndDate = newEndDate,
                        Money = premiumRequest.Money,
                        LastNotifiedTwoDaysBefore = null,
                        LastNotifiedOneHourBefore = null,
                    };

                    var addedPremiumUser = await _premiumUserRepository.Add(premiumUser);
                    await SendPremiumSubscriptionUpgradeEmail(user, newEndDate, isRenewal: false);

                    return addedPremiumUser;
                }
                else
                {
                    if (existingPremiumUser.EndDate >= DateTime.UtcNow)
                    {
                        // Extend current premium subscription
                        newEndDate = existingPremiumUser.EndDate.AddDays(premiumRequest.DurationInDays);
                    }
                    else
                    {
                        // Renew expired subscription
                        newEndDate = DateTime.UtcNow.AddDays(premiumRequest.DurationInDays);
                        existingPremiumUser.StartDate = DateTime.UtcNow; // Reset start date to now
                    }

                    existingPremiumUser.EndDate = newEndDate;
                    existingPremiumUser.Money = premiumRequest.Money;
                    existingPremiumUser.LastNotifiedOneHourBefore = null;
                    existingPremiumUser.LastNotifiedTwoDaysBefore = null;

                    await _premiumUserRepository.Update(existingPremiumUser);
                    await SendPremiumSubscriptionUpgradeEmail(user, newEndDate, isRenewal: true);

                    return existingPremiumUser;
                }
            }
            catch (NoSuchUserExistException ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            catch (UnableToUpdateUserException ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to upgrade user to premium");
                throw;
            }
        }


        public async Task<bool> DowngradePremiumUser(int userId)
        {
            try
            {
                var user = await GetUserEntityById(userId);
                var premiumUser = await GetPremiumUserByUserId(userId);

                ValidateUserIsPremium(user, premiumUser);
                CheckSubscriptionStatus(premiumUser);

                await DowngradeUserToNormal(user, premiumUser);

                return true;
            }
            catch (NoSuchUserExistException ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            catch (NoSuchPremiumUserExistException ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            catch (UnableToUpdateUserException ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            catch (UnableToDeleteUserException ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            catch (ActiveSubscriptionException ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downgrading user to normal.");
                throw;
            }
        }


        #endregion



        #region Private Methods

        private async Task<User> GetUserEntityById(int userId)
        {
            var user = await _userRepository.GetById(userId);
            if (user == null)
            {
                throw new NoSuchUserExistException($"User with ID {userId} does not exist.");
            }
            return user;
        }

        public async Task<PremiumUser> GetPremiumUserByUserId(int userId)
        {
            var premiumUser = await _premiumUserRepository.GetByUserId(userId);
            if (premiumUser == null)
            {
                throw new NoSuchPremiumUserExistException("The user is not a premium user.");
            }
            return premiumUser;
        }

        private void ValidateUserIsPremium(User user, PremiumUser premiumUser)
        {
            if (user.Role != RoleType.PremiumUser || premiumUser == null)
            {
                throw new NoSuchPremiumUserExistException("The user is not a premium user.");
            }
        }

        private void CheckSubscriptionStatus(PremiumUser premiumUser)
        {
            if (premiumUser.EndDate > DateTime.UtcNow)
            {
                throw new ActiveSubscriptionException("The user still has an active premium subscription.");
            }
        }

        private async Task DowngradeUserToNormal(User user, PremiumUser premiumUser)
        {
            user.Role = RoleType.NormalUser;
            await _userRepository.Update(user);
            await _premiumUserRepository.Delete(premiumUser.Id);
            _logger.LogInformation($"User with ID {user.UserId} downgraded to NormalUser.");
        }

        private async Task SendPremiumSubscriptionUpgradeEmail(User user, DateTime endDate, bool isRenewal)
        {
            string subject;
            string body;

            if (isRenewal)
            {
                subject = "VibeVault - Premium Subscription Renewed!";
                body = $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset='UTF-8'>
                        <title>VibeVault Premium Subscription Renewal</title>
                        <style>
                            body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; color: #333; margin: 0; padding: 0; }}
                            .container {{ max-width: 600px; margin: 50px auto; background-color: #fff; border-radius: 8px; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1); overflow: hidden; }}
                            .header {{ background-color: #FF5733; color: white; padding: 20px; text-align: center; }}
                            .content {{ padding: 30px; }}
                            .footer {{ padding: 20px; font-size: 12px; color: #777; text-align: center; background-color: #f9f9f9; }}
                            .premium-banner {{ font-size: 24px; font-weight: bold; margin: 20px 0; color: #FF5733; }}
                            .info-text {{ color: #555; }}
                            .social-links a {{ text-decoration: none; color: #FF5733; }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                <h1>VibeVault Premium Subscription Renewal</h1>
                            </div>
                            <div class='content'>
                                <p>Hello {user.Username},</p>
                                <p>Thank you for renewing your VibeVault Premium subscription! 🎉</p>
                                <div class='premium-banner'>Enjoy Premium Features Until {endDate:MMMM dd, yyyy}!</div>
                                <p>We're thrilled to have you continue enjoying your premium benefits, which include:</p>
                                <ol>
                                    <li>Unlimited playlists</li>
                                    <li>Unlimited songs in a playlist</li>
                                    <li>Keep the playlist, even after the subscription</li>
                                    <li>Offline downloads (coming soon...)</li>
                                    <li>And much more!</li>
                                </ol>
                                <br/>
                                <p class='info-text'>Your premium subscription is now extended until <strong>{endDate:MMMM dd, yyyy}</strong>. We're excited to have you explore the full potential of VibeVault's music experience.</p>
                                <p>For any questions or assistance, please visit our <a href='https://example.com/support'>Support Center</a>.</p>
                                <p>We hope you continue to enjoy your time with VibeVault. Keep vibing to your favorite tunes!</p>
                            </div>
                            <div class='footer'>
                                <p>Thank you for being a valued member of the VibeVault community!</p>
                                <p class='social-links'>
                                    Follow us: 
                                    <a href='https://twitter.com/vibevault'>Twitter</a> | 
                                    <a href='https://facebook.com/vibevault'>Facebook</a> | 
                                    <a href='https://instagram.com/vibevault'>Instagram</a>
                                </p>
                                <p>© 2024 VibeVault, All Rights Reserved</p>
                                <p>123 Music Street, Suite 400, Chennai, India</p>
                            </div>
                        </div>
                    </body>
                    </html>";
            }
            else
            {
                subject = "VibeVault - Welcome to Premium Membership!";
                body = $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset='UTF-8'>
                        <title>VibeVault Premium Membership</title>
                        <style>
                            body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; color: #333; margin: 0; padding: 0; }}
                            .container {{ max-width: 600px; margin: 50px auto; background-color: #fff; border-radius: 8px; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1); overflow: hidden; }}
                            .header {{ background-color: #4CAF50; color: white; padding: 20px; text-align: center; }}
                            .content {{ padding: 30px; }}
                            .footer {{ padding: 20px; font-size: 12px; color: #777; text-align: center; background-color: #f9f9f9; }}
                            .premium-banner {{ font-size: 24px; font-weight: bold; margin: 20px 0; color: #4CAF50; }}
                            .info-text {{ color: #555; }}
                            .social-links a {{ text-decoration: none; color: #4CAF50; }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                <h1>VibeVault Premium Membership</h1>
                            </div>
                            <div class='content'>
                                <p>Hello {user.Username},</p>
                                <p>Congratulations on becoming a VibeVault Premium Member! 🎉</p>
                                <div class='premium-banner'>Enjoy Premium Features Until {endDate:MMMM dd, yyyy}!</div>
                                <p>As a premium member, you now have access to exclusive features including:</p>
                                <ul>
                                    <li>Unlimited playlists</li>
                                    <li>Unlimited songs in a playlist</li>
                                    <li>Keep the playlist, even after the subscription</li>
                                    <li>Offline downloads (coming soon...)</li>
                                    <li>And much more!</li>
                                </ul>
                                <br/>
                                <p class='info-text'>Your premium subscription is valid until <strong>{endDate:MMMM dd, yyyy}</strong>. We're excited to have you explore the full potential of VibeVault's music experience.</p>
                                <p>For any questions or assistance, please visit our <a href='https://example.com/support'>Support Center</a>.</p>
                                <p>We hope you enjoy your time with VibeVault. Keep vibing to your favorite tunes!</p>
                            </div>
                            <div class='footer'>
                                <p>Thank you for being a valued member of the VibeVault community!</p>
                                <p class='social-links'>
                                    Follow us: 
                                    <a href='https://twitter.com/vibevault'>Twitter</a> | 
                                    <a href='https://facebook.com/vibevault'>Facebook</a> | 
                                    <a href='https://instagram.com/vibevault'>Instagram</a>
                                </p>
                                <p>© 2024 VibeVault, All Rights Reserved</p>
                                <p>123 Music Street, Suite 400, Chennai, India</p>
                            </div>
                        </div>
                    </body>
                    </html>";
            }

            await _emailSenderService.SendEmailAsync(user.Email, subject, body);
        }


        #endregion

    }
}
