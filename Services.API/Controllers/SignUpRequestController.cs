using App.Services.Dtos;
using App.Services.Services.SignUpRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.API.Attributes;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Services.API.Controllers
{
    [SwaggerTag("Manages SignUpRequest.")]
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpRequestController : Controller
    {
        private readonly ISignUpRequestService mySignUpRequestService;


        public SignUpRequestController(ISignUpRequestService signUpRequestService)
        {
            mySignUpRequestService = signUpRequestService;
        }

        /// <summary>
        /// Creates a Sign Up Request Request.
        /// </summary>
        /// <param name="signUpRequestDto"> User to be registered</param>
        /// <returns>Returns the created sign up request</returns>
        [HttpPost]
        [SwaggerOperation(Description = "Create a signup request.")]
        [ProducesResponseType(typeof(SignUpRequestResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status503ServiceUnavailable)]
        public async Task<ActionResult<SignUpRequestResponseDto>> Post(CreateSignUpRequestRequestDto signUpRequestDto)
        {


            // Add the resgistration request to the database, using the service
            var response = await mySignUpRequestService.CreateSignUpRequest(signUpRequestDto);

            return Ok(response);
        }


        /// <summary>
        /// Retrieves a signUpRequest by its identification
        /// </summary>
        /// <param name="id">signUp Request Id</param>
        /// <param name="iamId">signUp Request IamId</param>
        /// <returns>Returns a signUpRequest with the specified Id</returns> 
        [HttpGet("ids")]
        [SwaggerOperation(Description = "Retrieves a SignUpRequest by its identification.")]
        [ProducesResponseType(typeof(SignUpRequestResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status503ServiceUnavailable)]
        public async Task<ActionResult<SignUpRequestResponseDto>> Get([FromQuery(Name = "id")] string id, [FromQuery(Name = "iamId")] string iamId)
        {
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(iamId))
            {
                return BadRequest("Only one of the identifiers can be specified");
            }

            // Choose the parameter based on which one is provided
            var response = !string.IsNullOrEmpty(id) ? await mySignUpRequestService.GetByIdAsync(id.ToString()) : await mySignUpRequestService.GetByIamIdAsync(iamId.ToString());

            if (response == null)
            {
                return NotFound($"SignUpRequest not found");
            }


            return Ok(response);
        }


        /// <summary>
        /// Retrieves the state of a signUpRequest by its identification
        /// </summary>

        /// <param name="iamId">signUp Request IamId</param>
        /// <returns>Returns a signUpRequest with the specified Id</returns> 
        [HttpGet("state")]
        [SwaggerOperation(Description = "Retrieves the state of a signUpRequest by its identification.")]
        [ProducesResponseType(typeof(SignUpRequestResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status503ServiceUnavailable)]
        public async Task<ActionResult<StateOutDto>> Get([FromQuery(Name = "iamId")] string iamId)
        {

            // Choose the parameter based on which one is provided
            var response = await mySignUpRequestService.GetSignUpRequestState(iamId);

            return Ok(response);
        }


        /// <summary>
        /// Retrieves signUpRequests by its state
        /// </summary>
        /// <param name="state">signUpRequest state</param>
        /// <returns>Returns signUpRequests by state</returns>
        [HttpGet]
        [Authorize("manage:users")]
        [SwaggerOperation(Description = "Retrieves SignUpRequests by state.")]
        [ProducesResponseType(typeof(RegistrationRequestResponseList), StatusCodes.Status200OK)]
        public async Task<ActionResult<RegistrationRequestResponseList>> GetByState([FromQuery(Name = "state")] string state)
        {
            // Call your service to get the list of SignUpRequestResponseDto
            var response = await mySignUpRequestService.GetByStateAsync(state);

            return Ok(response);
        }

        /// <summary>
        /// Accepts or Rejects a SignUpRequest by its identification
        /// </summary>
        /// <param name="id">User Id</param>
        /// <param name="signUpRequestActionDto">Value for the management</param>
        /// <returns>Returns the created user</returns>
        [Authorize("manage:users")]
        [HttpPatch("{id}")]
        [SwaggerOperation(Description = "Accepts or Rejects a SignUpRequest by its identification.")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status503ServiceUnavailable)]
        public async Task<ActionResult<SignUpRequestResponseDto>> Patch([Required][GuidParse] string id, [FromBody] SignUpRequestActionDto signUpRequestActionDto)
        {
            // Accept or Reject the SignUpRequest
            var response = await mySignUpRequestService.AcceptOrRejectSignUpRequest(id, signUpRequestActionDto);

            return Ok(response);
        }

    }
}
