using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicApplicationAPI.Exceptions.UserExceptions;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Interfaces.Service.AuthService;
using MusicApplicationAPI.Models.DTOs.UserDTO;
using StudentManagementApplicationAPI.Models.ErrorModels;
using WatchDog;

namespace MusicApplicationAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class usersController : ControllerBase
    {
        #region Private Fields

        private readonly IUserService _userService;
        private readonly IAuthLoginService<UserLoginReturnDTO, UserLoginDTO> _authLoginService;
        private readonly IAuthRegisterService<UserRegisterReturnDTO, UserRegisterDTO> _authRegisterService;
        private readonly ILogger<usersController> _logger;

        #endregion

        #region Constructor

        public usersController(IUserService userService, ILogger<usersController> logger, IAuthRegisterService<UserRegisterReturnDTO, UserRegisterDTO> authRegisterService, IAuthLoginService<UserLoginReturnDTO, UserLoginDTO> authLoginService)
        {
            _userService = userService;
            _logger = logger;
            _authRegisterService = authRegisterService;
            _authLoginService = authLoginService;
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Updates a user's profile.
        /// </summary>
        /// <param name="user">The user data to be updated.</param>
        /// <param name="userId">The ID of the user to be updated.</param>
        /// <returns>The updated user data.</returns>
        [HttpPut("{userId}")]
        [ProducesResponseType(typeof(UserReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UserUpdateDTO user, [FromRoute] int userId)
        {
            if (user == null || userId <= 0)
            {
                return BadRequest(new ErrorModel(400, "Invalid user data or user ID."));
            }

            try
            {
                var result = await _userService.UpdateUserProfile(user, userId);
                return Ok(result);
            }
            catch (NoSuchUserExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (UnableToUpdateUserException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve.</param>
        /// <returns>The user data.</returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(UserReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserById([FromRoute] int userId)
        {
            if (userId <= 0)
            {
                return BadRequest(new ErrorModel(400, "Invalid user ID."));
            }

            try
            {
                var result = await _userService.GetUserById(userId);
                return Ok(result);
            }
            catch(NoSuchUserExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves a user by their email.
        /// </summary>
        /// <param name="email">The email of the user to retrieve.</param>
        /// <returns>The user data.</returns>
        [HttpGet("email")]
        [ProducesResponseType(typeof(UserReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new ErrorModel(400, "Email is required."));
            }

            try
            {
                var result = await _userService.GetUserByEmail(email);

                return Ok(result);
            }
            catch (NoSuchUserExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>A list of all users.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var result = await _userService.GetAllUsers();
                return Ok(result);
            }
            catch (NoUsersExistsExistsException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves all admin users.
        /// </summary>
        /// <returns>A list of all admin users.</returns>
        [HttpGet("admin")]
        [ProducesResponseType(typeof(IEnumerable<UserReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllAdminUsers()
        {
            try
            {
                var result = await _userService.GetAllAdminUsers();
                return Ok(result);
            }
            catch (NoUsersExistsExistsException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <returns>A message indicating the result of the deletion.</returns>
        [HttpDelete("{userId}")]
        [ProducesResponseType(typeof(UserReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser([FromRoute] int userId)
        {
            if (userId <= 0)
            {
                return BadRequest(new ErrorModel(400, "Invalid user ID."));
            }

            try
            {
                var result = await _userService.DeleteUser(userId);
                return Ok(result);
            }
            catch (NoSuchUserExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (UnableToDeleteUserException ex)
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
                var result = await _authRegisterService.Register(userRegisterDTO);
                return CreatedAtAction(nameof(GetUserById), new { userId = result.UserId }, result);
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

        #endregion
    }
}
