using App.Services.Dtos;
using App.Services.Repositories.Shared;
using Domain.Shared;
using IAM.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Services.API.Controllers
{
    /// <summary>
    /// Error Controller
    /// </summary>
    [Route("Error")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        /// <summary>
        /// Handles the exception and returns a response with the error.
        /// </summary>
        /// <returns></returns>
        public ActionResult<ErrorController> Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context.Error;
            var response = HandleException(exception);
            return response;
        }

        private ActionResult<ErrorController> HandleException(Exception exception)
        {
            ObjectResult response;
            var error = new Error();

            switch (exception)
            {
                case ArgumentNullException:
                    error.Message = exception.Message;
                    response = BadRequest(error);
                    break;
                case ArgumentException:
                    error.Message = exception.Message;
                    response = BadRequest(error);
                    break;
                case BusinessRuleValidationException:
                    error.Message = exception.Message;
                    response = BadRequest(error);
                    break;
                case EntityNotFoundException:
                    error.Message = exception.Message;
                    response = NotFound(error);
                    break;
                case EntityAlreadyExistsException:
                    error.Message = exception.Message;
                    response = Conflict(error);
                    break;
                case Auth0InvalidTokenException:
                    error.Message = exception.Message;
                    response = Unauthorized(error);
                    break;
                case Auth0BadRequestException:
                    error.Message = exception.Message;
                    response = BadRequest(error);
                    break;
                case Auth0InsufficientScopeException:
                    error.Message = exception.Message;
                    response = StatusCode(403, error);
                    break;
                case Auth0UserNotFoundException:
                    error.Message = exception.Message;
                    response = NotFound(error);
                    break;
                case Auth0UserAlreadyExistsException:
                    error.Message = exception.Message;
                    response = Conflict(error);
                    break;
                case Auth0TooManyRequestsException:
                    error.Message = exception.Message;
                    response = StatusCode(429, error);
                    break;
                case Exception:
                    error.Message = exception.Message;
                    response = StatusCode(500, error);
                    break;
                default:
                    error.Message = "Database Error";
                    response = StatusCode(503, error);
                    break;
            }

            return response;
        }

    }



}
