using AutoMapper;
using Easy_Password_Validator;
using MusicApplicationAPI.Exceptions.UserExceptions;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Interfaces.Service.AuthService;
using MusicApplicationAPI.Interfaces.Service.TokenService;
using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Models.Enums;
using MusicApplicationAPI.Models.DTOs.UserDTO;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using MusicApplicationAPI.Exceptions.EmailExceptions;

namespace MusicApplicationAPI.Services.UserService
{
    public class UserAuthService : IAuthLoginService<UserLoginReturnDTO, UserLoginDTO>,
        IAuthRegisterService<UserRegisterReturnDTO, UserRegisterDTO>
    {

        #region Private Fields

        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<UserAuthService> _logger;
        private readonly PasswordValidatorService _passwordValidatorService;

        private readonly bool AllowPasswordValidation;

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
            PasswordValidatorService passwordValidatorService,
            IConfiguration configuration
            )
        {
            _tokenService = tokenService;
            _userRepo = userRepo;
            _mapper = mapper;
            _logger = logger;
            _passwordValidatorService = passwordValidatorService;
            bool.TryParse(configuration.GetSection("AllowPasswordValidation").Value, out bool allowPasswordValidation);
            AllowPasswordValidation = allowPasswordValidation;
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
                var userInDB = await _userRepo.GetUserByEmail(userLoginDTO.Email);
                if (userInDB == null)
                {
                    throw new UnauthorizedUserException("Invalid username or password");
                }
                HMACSHA512 hMACSHA = new HMACSHA512(userInDB.PasswordHashKey);
                var encryptedPassword = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(userLoginDTO.Password));
                bool isPasswordSame = ComparePassword(encryptedPassword, userInDB.PasswordHash);
                if (isPasswordSame)
                {
                    if (userInDB.Status != null && userInDB.Status.ToLower() == "active")
                    {
                        UserLoginReturnDTO loginReturnDTO = new UserLoginReturnDTO()
                        {
                            Email = userInDB.Email,
                            UserId = userInDB.UserId,
                            Token = _tokenService.GenerateToken(userInDB),
                            Role = userInDB.Role,
                            Username = userInDB.Username
                        };
                        return loginReturnDTO;
                    }
                    throw new EmailNotVerifiedException("You haven't verified your email");
                }
                else
                {
                    throw new UnauthorizedUserException("Invalid username or password");
                }
            }
            catch (UnauthorizedUserException ex)
            {
                _logger.LogError(ex.Message);
                throw new UnauthorizedUserException("Invalid username or password");
            }
            catch (EmailNotVerifiedException ex)
            {
                _logger.LogError(ex.Message);
                throw new EmailNotVerifiedException("You havent verified your email, verify it first");
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

                #region Password Validation
                if (AllowPasswordValidation)
                {
                    var isPasswordValid = _passwordValidatorService.TestAndScore(userRegisterDTO.Password);
                    Debug.WriteLine($"Password score {_passwordValidatorService.Score}, {_passwordValidatorService.FailureMessages} ");
                    var failureMessages = string.Join(", ", _passwordValidatorService.FailureMessages);
                    if (!isPasswordValid)
                    {
                        Debug.WriteLine("Invalid password");
                        throw new InvalidPasswordException(failureMessages);
                    }
                }
                #endregion

                DateTime dateOfBirth = userRegisterDTO.DOB.ToDateTime(TimeOnly.MinValue);

                user = new User()
                {
                    Username = userRegisterDTO.Username,
                    Email = userRegisterDTO.Email,
                    DOB = dateOfBirth,
                    Role = RoleType.NormalUser,
                    Status = "Not Verified",
                };

                HMACSHA512 hMACSHA = new HMACSHA512();
                user.PasswordHashKey = hMACSHA.Key;
                user.PasswordHash = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(userRegisterDTO.Password));

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

        /// <summary>
        /// Compares two passwords to check if they are the same.
        /// </summary>
        /// <param name="encryptedPassword">The encrypted password.</param>
        /// <param name="userPassword">The stored user password.</param>
        /// <returns>True if the passwords match, otherwise false.</returns>

        private bool ComparePassword(byte[] encryptedPassword, byte[] userPassword)
        {
            if (encryptedPassword.Length != userPassword.Length)
            {
                return false;
            }
            for (int i = 0; i < encryptedPassword.Length; i++)
            {
                if (encryptedPassword[i] != userPassword[i])
                {
                    return false;
                }
            }
            return true;
        }

    }
}
