using App.Services.Dtos;
using App.Services.Services.Auth;
using App.Services.Services.SignUpRequests;
using Auth0.AuthenticationApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.API.Attributes;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Threading.Tasks;

namespace Services.API.Controllers
{
    [SwaggerTag("Manages Authentication.")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService myAuthService;

        public AuthController(IAuthService authSerivce)
        {
            myAuthService = authSerivce;
        }


        /// <summary>
        /// Retrieves the role of a user by its iam ID
        /// </summary>
        /// <param name="iamId">iam id of logged user</param>
        /// <returns>Retrieves the role of a user by its iam ID</returns>  

        [HttpGet("{iamId}")]
        [SwaggerOperation(Description = "Retrieves the role of a user by its iam ID.")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status503ServiceUnavailable)]
        public async Task<ActionResult<SignUpRequestResponseDto>> Get([Required] string iamId)
        {
            Console.WriteLine($"Received IAM ID: {iamId}");

            // Get the user from the database
            var role = await myAuthService.VerifyIfUserExists(iamId);

            var response = new
            {
                role = role,
                iamId = iamId
            };

            // Return the user
            return Ok(response);
        }


    }
}
