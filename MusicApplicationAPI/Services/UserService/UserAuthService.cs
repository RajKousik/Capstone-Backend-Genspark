using AutoMapper;
using MusicApplicationAPI.Exceptions.UserExceptions;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Interfaces.Service.AuthService;
using MusicApplicationAPI.Interfaces.Service.TokenService;
using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Models.Enums;
using MusicApplicationAPI.Models.DTOs.UserDTO;
using MusicApplicationAPI.Exceptions.EmailExceptions;
using MusicApplicationAPI.Interfaces.Service;

namespace MusicApplicationAPI.Services.UserService
{
    public class UserAuthService : IAuthLoginService<UserLoginReturnDTO, UserLoginDTO>,
        IAuthRegisterService<UserRegisterReturnDTO, UserRegisterDTO>
    {

        #region Private Fields

        private readonly ITokenService _tokenService;
        private readonly IPremiumUserRepository _premiumUserRepository;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<UserAuthService> _logger;
        private readonly IPasswordService _passwordService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAuthService"/> class.
        /// </summary>
        /// <param name="tokenService">The token service for generating JWT tokens.</param>
        /// <param name="userRepo">The repository for user data.</param>
        /// <param name="mapper">The mapper for transforming DTOs and models.</param>

        public UserAuthService(
            ITokenService tokenService,
            IUserRepository userRepo,
            IMapper mapper,
            ILogger<UserAuthService> logger,
            IPasswordService passwordService,
            IPremiumUserRepository premiumUserRepository
            )
        {
            _tokenService = tokenService;
            _userRepo = userRepo;
            _mapper = mapper;
            _logger = logger;
            _passwordService = passwordService;
            _premiumUserRepository = premiumUserRepository;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Logs in a user and generates a JWT token if the credentials are valid.
        /// </summary>
        /// <param name="userLoginDTO">The login data transfer object containing email and password.</param>
        /// <returns>A <see cref="UserLoginReturnDTO"/> containing the login result and token.</returns>
        /// <exception cref="UnauthorizedUserException">Thrown when the credentials are invalid or the account is not activated.</exception>
        public async Task<UserLoginReturnDTO> Login(UserLoginDTO userLoginDTO)
        {
            try
            {
                var userInDB = await GetUserByEmail(userLoginDTO.Email);
                ValidateUserCredentials(userLoginDTO.Password, userInDB);
                EnsureUserIsActive(userInDB);

                bool isPremiumExpired = await CheckPremiumSubscription(userInDB);

                if (isPremiumExpired)
                {
                    var shortLivedToken = _tokenService.GenerateShortLivedToken(userInDB);
                    return new UserLoginReturnDTO
                    {
                        Email = userInDB.Email,
                        UserId = userInDB.UserId,
                        Token = shortLivedToken,
                        Role = userInDB.Role,
                        Username = userInDB.Username,
                        IsPremiumExpired = true,
                        Message = "Your premium subscription has expired. Please renew to continue enjoying premium features."
                    };
                }
                else
                {
                    return GenerateLoginReturnDTO(userInDB);
                }
            }
            catch (EmailNotVerifiedException ex)
            {
                _logger.LogError(ex.Message);
                throw new EmailNotVerifiedException("You haven't verified your email, verify it first");
            }
            catch (UnauthorizedUserException ex)
            {
                _logger.LogError(ex.Message);
                throw new UnauthorizedUserException("Invalid username or password");
            }
            catch (PremiumSubscriptionExpiredException ex)
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


        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userRegisterDTO">The registration data transfer object containing user details.</param>
        /// <returns>A <see cref="UserRegisterReturnDTO"/> containing the registered user's details.</returns>
        /// <exception cref="UnableToAddUserException">Thrown when the user could not be added to the database.</exception>
        /// <exception cref="DuplicateEmailException">Thrown when the email is already registered.</exception>
        /// <exception cref="Exception">Thrown when an unknown error occurs during registration.</exception>
        public async Task<UserRegisterReturnDTO> Register(UserRegisterDTO userRegisterDTO)
        {
            User user;
            try
            {
                var emailExists = await _userRepo.GetUserByEmail(userRegisterDTO.Email);
                if (emailExists != null)
                {
                    throw new DuplicateEmailException("Email id is already registered");
                }

                DateTime dateOfBirth = userRegisterDTO.DOB.ToDateTime(TimeOnly.MinValue);

                user = new User()
                {
                    Username = userRegisterDTO.Username,
                    Email = userRegisterDTO.Email,
                    DOB = dateOfBirth,
                    Role = RoleType.NormalUser,
                    Status = "Not Verified",
                };

                user.PasswordHash = _passwordService.HashPassword(userRegisterDTO.Password, out byte[] key);
                user.PasswordHashKey = key;

                var addedUser = await _userRepo.Add(user);

                UserRegisterReturnDTO userRegisterReturnDTO = _mapper.Map<UserRegisterReturnDTO>(addedUser);

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

        #endregion

        #region Private Methods

        private async Task<User> GetUserByEmail(string email)
        {
            var user = await _userRepo.GetUserByEmail(email);
            if (user == null)
            {
                throw new UnauthorizedUserException("Invalid username or password");
            }
            return user;
        }

        private void ValidateUserCredentials(string password, User user)
        {
            if (!_passwordService.VerifyPassword(password, user.PasswordHash, user.PasswordHashKey))
            {
                throw new UnauthorizedUserException("Invalid username or password");
            }
        }

        private void EnsureUserIsActive(User user)
        {
            if (user.Status == null || user.Status.ToLower() != "active")
            {
                throw new EmailNotVerifiedException("You haven't verified your email");
            }
        }

        private async Task<bool> CheckPremiumSubscription(User user)
        {
            var premiumUser = await _premiumUserRepository.GetByUserId(user.UserId);
            if (premiumUser != null && premiumUser.EndDate < DateTime.UtcNow)
            {
                return true; // Indicating premium has expired
            }
            return false;
        }

        private UserLoginReturnDTO GenerateLoginReturnDTO(User user)
        {
            return new UserLoginReturnDTO
            {
                Email = user.Email,
                UserId = user.UserId,
                Token = _tokenService.GenerateToken(user),
                Role = user.Role,
                Username = user.Username,
                IsPremiumExpired = false,
                Message = "Logged in"
            };
        }


        #endregion

    }
}
