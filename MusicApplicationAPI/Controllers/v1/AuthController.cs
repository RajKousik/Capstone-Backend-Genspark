using Microsoft.AspNetCore.Mvc;
using MusicApplicationAPI.Exceptions.EmailExceptions;
using MusicApplicationAPI.Exceptions.UserExceptions;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Interfaces.Service.AuthService;
using MusicApplicationAPI.Models.DTOs.UserDTO;
using MusicApplicationAPI.Models.ErrorModels;
using System.ComponentModel.DataAnnotations;
using WatchDog;

namespace MusicApplicationAPI.Controllers.v1
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        #region Private Fields
        private readonly IEmailSender _emailSenderService;
        private readonly IEmailVerificationService _emailVerificationService;
        private readonly IAuthLoginService<UserLoginReturnDTO, UserLoginDTO> _authLoginService;
        private readonly IAuthRegisterService<UserRegisterReturnDTO, UserRegisterDTO> _authRegisterService;
        private readonly ILogger<AuthController> _logger;
        #endregion


        #region Constructor

        public AuthController(ILogger<AuthController> logger, IAuthRegisterService<UserRegisterReturnDTO, UserRegisterDTO> authRegisterService, IAuthLoginService<UserLoginReturnDTO, UserLoginDTO> authLoginService, IEmailSender emailSenderService, IEmailVerificationService emailVerificationService)
        {
            _logger = logger;
            _authRegisterService = authRegisterService;
            _authLoginService = authLoginService;
            _emailSenderService = emailSenderService;
            _emailVerificationService = emailVerificationService;
        }

        #endregion

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="userLoginDTO">The user login data.</param>
        /// <returns>The login result including the token.</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(UserLoginReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            if (userLoginDTO == null)
            {
                return BadRequest(new ErrorModel(400, "Invalid login data."));
            }

            try
            {
                var result = await _authLoginService.Login(userLoginDTO);
                return Ok(result);
            }
            catch (UnauthorizedUserException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(401, new ErrorModel(401, $"{ex.Message}"));
            }
            catch (EmailNotVerifiedException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(401, new ErrorModel(401, $"{ex.Message}"));
            }
            catch (PremiumSubscriptionExpiredException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(401, new ErrorModel(401, $"{ex.Message}"));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorModel(500, $"{ex.Message}"));
            }
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userRegisterDTO">The user registration data.</param>
        /// <returns>The registration result.</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(UserRegisterReturnDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegisterDTO)
        {
            if (userRegisterDTO == null)
            {
                return BadRequest(new ErrorModel(400, "Invalid registration data."));
            }

            try
            {
                var registeredUser = await _authRegisterService.Register(userRegisterDTO);
                await _emailVerificationService.CreateEmailVerification(registeredUser.UserId);
                return StatusCode(201, registeredUser);
            }
            catch (DuplicateEmailException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(409, new ErrorModel(409, $"{ex.Message}"));
            }
            catch (InvalidPasswordException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(409, new ErrorModel(409, $"{ex.Message}"));
            }
            catch (UnableToAddUserException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorModel(500, $"{ex.Message}"));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorModel(409, $"{ex.Message}"));
            }
        }



        [HttpPost("verify/generate-verification-code")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GenerateVerificationCode(int userId)
        {
            try
            {
                await _emailVerificationService.CreateEmailVerification(userId);
                return Ok(new { message = "Verification code generated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        
        [HttpPost("verify/verify-code/{verificationCode}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> VerificationCode(int userId, [RegularExpression(@"^\d{6}$")] string verificationCode)
        {
            try
            {
                var isSuccess = await _emailVerificationService.VerifyEmail(userId, verificationCode);

                return Ok(new { message = "Verified successfully." });
            }
            catch (Exception ex)
            {
                if (ex is EmailVerificationNotFoundException || ex is InvalidEmailVerificationCodeException || ex is VerificationExpiredException)
                {
                    return BadRequest(new { message = ex.Message });
                }
                return StatusCode(500, new { error = ex.Message });
            }
        }

    }
}
