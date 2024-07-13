using App.Services.Dtos;
using App.Services.Services.Skeletons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.API.Attributes;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Services.API.Controllers
{
    /// <summary>
    /// Skeletons Controller
    /// </summary>
    [SwaggerTag("Manages Skeletons.")]
    [Route("api/[controller]")]
    [ApiController]
    public class SkeletonsController : Controller
    {
        private readonly ISkeletonService mySkeletonService;

        /// <summary>
        /// Initializes a SkeletonsController
        /// </summary>
        /// <param name="skeletonService">DI of the skeleton service</param>
        public SkeletonsController(ISkeletonService skeletonService)
        {
            mySkeletonService = skeletonService;
        }

        /// <summary>
        /// Retrieves a list of skeletons
        /// </summary>
        /// <returns>Returns a list of skeletons</returns>
        /// <response code="200"></response>  
        [HttpGet]
        [SwaggerOperation(Description = "Retrieves a list of skeletons.")]
        [ProducesResponseType(typeof(SkeletonResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status503ServiceUnavailable)]
        public async Task<ActionResult<SkeletonResponseList>> Get()
        {
            // Get the skeletons
            var response = await mySkeletonService.GetSkeletonsAsync();

            return Ok(response);
        }


        /// <summary>
        /// Retrieves a skeleton by its identification
        /// </summary>
        /// <param name="id">Skeleton Id</param>
        /// <returns>Returns a Skeleton with the specified Id</returns>  
        [HttpGet("{id}")]
        [SwaggerOperation(Description = "Retrieves a skeleton by its identification.")]
        [ProducesResponseType(typeof(SkeletonResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status503ServiceUnavailable)]
        public async Task<ActionResult<SkeletonResponse>> Get([Required][GuidParse] string id)
        {
            // Get the skeleton with such id
            var response = await mySkeletonService.GetByIdAsync(id);

            // In this case, when the skeleton is not found an exception is thrown, so we don't need to check if it is null to return a 404
            // But this is just to show how to use the exception, normally in the service you would return null and check it here to return a 404 like:
            // if (response == null) return NotFound("No skeleton found with such id");


            return Ok(response);
        }

        /// <summary>
        /// Creates a skeleton.
        /// </summary>
        /// <param name="createSkeletonRequestDto"> Skeleton to be added</param>
        /// <returns>Returns the created skeleton</returns>
        [HttpPost]
        [SwaggerOperation(Description = "Creates a skeleton.")]
        [ProducesResponseType(typeof(SkeletonResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status503ServiceUnavailable)]
        public async Task<ActionResult<SkeletonResponse>> Post(CreateSkeletonRequestDto createSkeletonRequestDto)
        {
            // Add the skeleton to the database
            var response = await mySkeletonService.AddSkeletonAsync(createSkeletonRequestDto);

            return Ok(response);
        }
    }
}
