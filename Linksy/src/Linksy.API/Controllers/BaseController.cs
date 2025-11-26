using Linksy.API.API;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Linksy.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v{v:apiVersion}" + "/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected const string _notFoundTypeUrl = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5";
        protected readonly IMediator _mediator;

        protected BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }
        protected ActionResult<ApiResponse<T>> Ok<T>(T model)
        {
            return base.Ok(new ApiResponse<T>(HttpStatusCode.OK, model));
        }
        protected ActionResult<ApiResponse<TResponse>> OkOrNotFound<TResponse>(TResponse? model, string entityName)
        {
            if (model is not null)
            {
                return Ok(model);
            }
            return NotFound(new ProblemDetails()
            {
                Type = _notFoundTypeUrl,
                Title = $"{entityName} was not found.",
                Status = (int)HttpStatusCode.NotFound
            });
        }
        protected virtual ActionResult<ApiResponse<TResponse>> OkOrNotFound<TResponse, TEntityId>(TResponse? model, string entityName, TEntityId id)
        {
            if (model is not null)
            {
                return Ok(model);
            }
            return NotFound(new ProblemDetails()
            {
                Type = _notFoundTypeUrl,
                Title = $"{entityName} with ID: {id} was not found.",
                Status = (int)HttpStatusCode.NotFound
            });
        }
    }
}
