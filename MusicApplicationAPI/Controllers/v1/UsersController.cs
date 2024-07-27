using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using MusicApplicationAPI.Exceptions.UserExceptions;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Interfaces.Service.AuthService;
using MusicApplicationAPI.Models.DTOs.OtherDTO;
using MusicApplicationAPI.Models.DTOs.UserDTO;
using MusicApplicationAPI.Models.ErrorModels;
using MusicApplicationAPI.Services.UserService;
using WatchDog;

namespace MusicApplicationAPI.Controllers.v1
{
    [Route("api/v1/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        #region Private Fields

        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        #endregion

        #region Constructor

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
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
        [Authorize]
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
        [Authorize]
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
        /// Retrieves a user by their email.
        /// </summary>
        /// <param name="email">The email of the user to retrieve.</param>
        /// <returns>The user data.</returns>
        [HttpGet("email")]
        [Authorize]
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
        [Authorize]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize]
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
        /// Changes the password for a user.
        /// </summary>
        /// <param name="requestDTO">The password change request data.</param>
        /// <param name="userId">The ID of the user whose password is being changed.</param>
        /// <returns>A message indicating the result of the password change.</returns>
        [HttpPut("change-password")]
        [Authorize]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDTO requestDTO, int userId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _userService.ChangePassword(requestDTO, userId);
                if (result)
                    return Ok(new { message = "Password changed successfully." });
                else
                    return BadRequest(new { message = "Current password is incorrect." });
            }
            catch (NoSuchUserExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (InvalidPasswordException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(400, new ErrorModel(400, ex.Message));
            }
            catch (UnableToUpdateUserException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
            catch (Exception ex)
            {
                // Log exception details here
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Upgrades a user to Premium status.
        /// </summary>
        /// <param name="userId">The ID of the user to upgrade.</param>
        /// <param name="premiumRequestDTO">The details for upgrading to Premium.</param>
        /// <returns>An action result indicating success or failure.</returns>
        [Authorize]
        [HttpPut("upgrade-to-premium")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpgradeToPremium(int userId, [FromBody] PremiumRequestDTO premiumRequestDTO)
        {
            try
            {
                await _userService.UpgradeUserToPremium(userId, premiumRequestDTO);
                return Ok(new { Message = "User upgraded to Premium successfully." });
            }
            catch (NoSuchUserExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (UnableToUpdateUserException ex)
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
        /// Downgrades a premium user to a normal user.
        /// </summary>
        /// <param name="userId">The ID of the user to downgrade.</param>
        /// <returns>An action result indicating success or failure.</returns>
        [Authorize]
        [HttpPost("downgrade")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DowngradePremiumUser(int userId)
        {
            try
            {
                await _userService.DowngradePremiumUser(userId);
                return Ok(new { message = "User downgraded to normal user successfully." });
            }
            catch (NoSuchUserExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (ActiveSubscriptionException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(400, new ErrorModel(400, ex.Message));
            }
            catch (NoSuchPremiumUserExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
            }
            catch (UnableToUpdateUserException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorModel(500, ex.Message));
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

        #endregion
    }
}
