using App.Services.Dtos;
using App.Services.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.API.Attributes;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Services.API.Controllers
{
    /// <summary>
    /// Users Controller
    /// </summary>
    [SwaggerTag("Manages Users.")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserService myUserService;

        /// <summary>
        /// Initializes a UsersController
        /// </summary>
        /// <param name="userService">DI of the Users service</param>
        public UsersController(IUserService userService)
        {
            myUserService = userService;
        }

        /// <summary>
        /// Retrieves a list of users
        /// </summary>
        /// <returns>Returns a list of users</returns>
        /// <response code="200"></response>
        [HttpGet]
        [Authorize("read:users")]
        [SwaggerOperation(Description = "Retrieves a list of users.")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status503ServiceUnavailable)]
        public async Task<ActionResult<UserResponseList>> Get()
        {
            // Get the users
            var response = await myUserService.GetUsersAsync();

            return Ok(response);
        }


        /// <summary>
        /// Retrieves a user by its id or iamId
        /// </summary>
        /// <param name="id">User Id</param>
        /// <param name="iamId">User IamId</param>
        /// <returns>Returns a User with the specified Id or IamId</returns>  
        [HttpGet("ids")]
        [SwaggerOperation(Description = "Retrieves a user by its id or iamId.")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status503ServiceUnavailable)]
        public async Task<ActionResult<UserResponse>> Get([FromQuery(Name = "id")] string id, [FromQuery(Name = "iamId")] string iamId)
        {
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(iamId))
            {
                return BadRequest("Only one of the identifiers can be specified");
            }

            if (!string.IsNullOrEmpty(id))
            {
                // Get the user with the specified id

                var response = await myUserService.GetByIdAsync(id);
                if (response == null)
                {
                    return NotFound($"User with id {id} not found");
                }


                return Ok(response);
            }

            if (!string.IsNullOrEmpty(iamId))
            {
                // Get the user with the specified iamId
                var response = await myUserService.GetByIamIdAsync(iamId);
                if (response == null)
                {
                    return NotFound($"User with iamId {iamId} not found");
                }
                return Ok(response);
            }

            return BadRequest("No iamId or id specified");
        }

        /// <summary>
        /// Creates a user.
        /// </summary>
        /// <param name="createUserRequestDto"> User to be added</param>
        /// <returns>Returns the created user</returns>
        [Authorize("manage:users")]
        [HttpPost]
        [SwaggerOperation(Description = "Creates a user.")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status503ServiceUnavailable)]
        public async Task<ActionResult<UserResponse>> Post(CreateManagerUserRequestDto createUserRequestDto)
        {
            // Add the user to the database
            var response = await myUserService.AddUserAsync(createUserRequestDto);

            // Return the created user with code 201
            return CreatedAtAction(nameof(Post), new { id = response.Id }, response);

        }

        /// <summary>
        /// Deletes a user by its identification
        /// </summary>
        /// <param name="iamId"></param>
        /// <returns></returns>
        [Authorize("user:requests")]
        [HttpDelete("{iamId}")]
        [SwaggerOperation(Description = "Deletes a user by its identification.")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status503ServiceUnavailable)]
        public async Task<ActionResult<UserResponse>> Delete(string iamId)
        {
            // Delete the user from the database
            var response = await myUserService.RemoveUserAsync(iamId);

            return Ok(response);
        }

        /// <summary>
        /// Retrieves a list of roles
        /// </summary>
        /// <returns></returns>
        [Authorize("manage:users")]
        [HttpGet("ManagerRoles")]
        [SwaggerOperation(Description = "Retrieves a list of roles.")]
        [ProducesResponseType(typeof(RoleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status503ServiceUnavailable)]
        public async Task<ActionResult<RoleResponseList>> GetManagerRoles()
        {
            // Get the roles
            var response = await myUserService.GetManagerRolesAysnc();

            return Ok(response);
        }


        /// <summary>
        /// Updates a user by its identification
        /// </summary>
        /// <param name="id">User Id</param>
        /// <param name="updateUserDto">User details to be updated</param>
        /// <returns>Returns the updated user</returns>
        [Authorize("user:requests")]
        [HttpPatch("{id}")]
        [SwaggerOperation(Description = "Updates a user by its identification.")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status503ServiceUnavailable)]
        public async Task<ActionResult<UserResponse>> Patch([Required][GuidParse] string id, [FromBody] UpdateUserRequestDto updateUserDto)
        {

            // Save the updated user to the database
            var response = await myUserService.UpdateUserAsync(id, updateUserDto);

            return Ok(response);
        }
    }
}
