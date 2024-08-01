using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MusicApplicationAPI.Exceptions.ArtistExceptions;
using MusicApplicationAPI.Exceptions.EmailExceptions;
using MusicApplicationAPI.Exceptions.UserExceptions;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Interfaces.Service.AuthService;
using MusicApplicationAPI.Models.DTOs.ArtistDTO;
using MusicApplicationAPI.Models.DTOs.UserDTO;
using MusicApplicationAPI.Models.ErrorModels;
using MusicApplicationAPI.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
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
        private readonly IArtistService _artistService;
        #endregion


        #region Constructor

        public AuthController(ILogger<AuthController> logger, IAuthRegisterService<UserRegisterReturnDTO, UserRegisterDTO> authRegisterService, IAuthLoginService<UserLoginReturnDTO, UserLoginDTO> authLoginService, IEmailSender emailSenderService, IEmailVerificationService emailVerificationService, IArtistService artistService)
        {
            _logger = logger;
            _authRegisterService = authRegisterService;
            _authLoginService = authLoginService;
            _emailSenderService = emailSenderService;
            _emailVerificationService = emailVerificationService;
            _artistService = artistService;
        }

        #endregion

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="userLoginDTO">The user login data.</param>
        /// <returns>The login result including the token.</returns>
        [HttpPost("user/login")]
        [EnableCors("MyAllowSpecificOrigins")]
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
                Response.Cookies.Append("vibe-vault", result.Token, new CookieOptions
                {
                    Secure = true,
                    HttpOnly = true,
                    SameSite = SameSiteMode.None,
                    MaxAge = TimeSpan.FromHours(8),
                    Expires = DateTimeOffset.UtcNow.AddHours(8)
                });
                return Ok(result);
            }
            catch (UnauthorizedUserException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(401, new ErrorModel(401, ex.Message));
            }
            catch (EmailNotVerifiedException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(401, new ErrorModel(401, ex.Message));
            }
            catch (PremiumSubscriptionExpiredException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(401, new ErrorModel(401, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userRegisterDTO">The user registration data.</param>
        /// <returns>The registration result.</returns>
        [HttpPost("user/register")]
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
                return StatusCode(409, new ErrorModel(409, ex.Message));
            }
            catch (InvalidPasswordException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(409, new ErrorModel(409, ex.Message));
            }
            catch (UnableToAddUserException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Logs out a user.
        /// </summary>
        /// <returns>The status code with message.</returns>
        [Authorize]
        [HttpPost("logout")]
        [EnableCors("MyAllowSpecificOrigins")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public IActionResult Logout()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.Name);
                if (Request.Cookies["vibe-vault"] != null)
                {
                    Response.Cookies.Delete("vibe-vault", new CookieOptions
                    {
                        HttpOnly = false,
                        SameSite = SameSiteMode.None,
                        Secure = true,
                        Expires = DateTimeOffset.UtcNow.AddMinutes(-1)
                    });
                }
                WatchLogger.Log($"{userId} Logged Out!");
                return Ok(new { Message = "Logged out successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Logs in an artist.
        /// </summary>
        /// <param name="artistLoginDTO">The artist login data.</param>
        /// <returns>The login result including the token.</returns>
        [HttpPost("artist/login")]
        [EnableCors("MyAllowSpecificOrigins")]
        [ProducesResponseType(typeof(ArtistLoginReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ArtistLogin([FromBody] ArtistLoginDTO artistLoginDTO)
        {
            if (artistLoginDTO == null)
            {
                return BadRequest(new ErrorModel(400, "Invalid login data."));
            }

            try
            {
                var result = await _artistService.Login(artistLoginDTO);
                Response.Cookies.Append("vibe-vault", result.Token, new CookieOptions
                {
                    Secure = true,
                    HttpOnly = true,
                    SameSite = SameSiteMode.None,
                    MaxAge = TimeSpan.FromHours(8),
                    Expires = DateTimeOffset.UtcNow.AddHours(8)
                });
                return Ok(result);
            }
            catch (UnauthorizedUserException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(401, new ErrorModel(401, ex.Message));
            }
            catch (ArtistNotActiveException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(401, new ErrorModel(401, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }



        /// <summary>
        /// Logs in an artist.
        /// </summary>
        /// <param name="artistLoginDTO">The artist login data.</param>
        /// <returns>The login result including the token.</returns>
        [HttpPut("artist/verify")]
        [Authorize(Roles ="Admin")]
        [ProducesResponseType(typeof(ArtistReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ArtistLogin([FromQuery] int artistId)
        {
            if (artistId <= 0)
            {
                return BadRequest(new ErrorModel(400, "Invalid data."));
            }

            try
            {
                var result = await _artistService.ActivateArtist(artistId);
                return Ok(result);
            }
            catch (UnauthorizedUserException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(401, new ErrorModel(401, ex.Message));
            }
            catch (NoSuchArtistExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (UnableToUpdateArtistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(400, new ErrorModel(400, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Registers a new artist.
        /// </summary>
        /// <param name="artistAddDTO">The artist registration data.</param>
        /// <returns>The registration result.</returns>
        [HttpPost("artist/register")]
        [ProducesResponseType(typeof(ArtistReturnDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ArtistRegister([FromBody] ArtistAddDTO artistAddDTO)
        {
            if (artistAddDTO == null)
            {
                return BadRequest(new ErrorModel(400, "Invalid registration data."));
            }

            try
            {
                var registeredArtist = await _artistService.Register(artistAddDTO);
                return StatusCode(201, registeredArtist);
            }
            catch (DuplicateEmailException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(409, new ErrorModel(409, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Generates an email verification code for a user.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <returns>The status code with a success message.</returns>
        [HttpPost("verify/generate-verification-code")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GenerateVerificationCode(int userId)
        {
            try
            {
                await _emailVerificationService.CreateEmailVerification(userId);
                return Ok(new { message = "Verification code generated successfully." });
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message ));
            }
        }

        /// <summary>
        /// Verifies the email verification code.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <param name="verificationCode">The email verification code.</param>
        /// <returns>The status code with a success message.</returns>
        [HttpPost("verify/verify-code")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> VerifyVerifiactionCode([FromBody] VerificationCodeDTO verificationCodeDTO)
        {
            try
            {
                var isSuccess = await _emailVerificationService.VerifyEmail(verificationCodeDTO.UserId, verificationCodeDTO.Code);

                return Ok(new { message = "Verified successfully." });
            }
            catch (EmailVerificationNotFoundException ex)
{
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorModel(500, $"{ex.Message}"));
            }
            catch (InvalidEmailVerificationCodeException ex)
{
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(400, new ErrorModel(400, $"{ex.Message}"));
            }
            catch (VerificationExpiredException ex)
{
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(400, new ErrorModel(400, $"{ex.Message}"));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorModel(500, $"{ex.Message}"));
            }
        }

    }
}
