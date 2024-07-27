using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicApplicationAPI.Exceptions.UserExceptions;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Models.DTOs.UserDTO;
using MusicApplicationAPI.Models.ErrorModels;
using WatchDog;

namespace MusicApplicationAPI.Controllers.v1
{
    [Route("api/v1/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        #region Private Fields
        private readonly ILogger<AdminController> _logger;
        private readonly IUserService _userService;
        #endregion

        #region Constructor
        public AdminController(ILogger<AdminController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }
        #endregion

        #region Public Endpoints

        /// <summary>
        /// Adds a new admin user.
        /// </summary>
        /// <param name="adminCreateDTO">The details of the new admin to be added.</param>
        /// <returns>An action result indicating success or failure.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserRegisterReturnDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddAdmin([FromBody] UserRegisterDTO adminCreateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _userService.AddAdmin(adminCreateDTO);
                return CreatedAtAction(nameof(GetAdminById), new { userId = result.UserId }, result);
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
        /// Retrieves a admin by their id.
        /// </summary>
        /// <returns>A Admin Return DTO.</returns>
        [HttpGet("{adminId}")]

        [ProducesResponseType(typeof(UserReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAdminById(int adminId)
        {
            try
            {
                var result = await _userService.GetAdminById(adminId);
                return Ok(result);
            }
            catch (NoUsersExistsExistsException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(404, new ErrorModel(404, ex.Message));
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
        /// Retrieves all admin users.
        /// </summary>
        /// <returns>A list of all admin users.</returns>
        [HttpGet]
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

        #endregion
    }
}
