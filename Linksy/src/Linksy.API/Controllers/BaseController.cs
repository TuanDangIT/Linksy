using Linksy.API.API;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Linksy.API.Controllers
{
    [ApiController]
    [Route("api/v{v:apiVersion}" + "/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        private const string _notFoundTypeUrl = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5";
        protected readonly IMediator _mediator;

        protected BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }
        protected ActionResult<ApiResponse<TResponse>> OkOrNotFound<TResponse>(TResponse? model, string entityName)
        {
            if (model is not null)
            {
                return Ok(new ApiResponse<TResponse>(HttpStatusCode.OK, model));
            }
            return NotFound(new ProblemDetails()
            {
                Type = _notFoundTypeUrl,
                Title = $"{entityName} was not found.",
                Status = (int)HttpStatusCode.NotFound
            });
        }
    }
}
