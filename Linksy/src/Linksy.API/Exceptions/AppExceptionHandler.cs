using Linksy.Application.Shared.Exceptions;
using Linksy.Domain.Exceptions;
using Linksy.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Linksy.API.Exceptions
{
    internal class AppExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<AppExceptionHandler> _logger;
        private readonly TimeProvider _timeProvider;
        private const string _badRequestTypeUrl = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1";
        private const string _serverErrorTypeUrl = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1";

        public AppExceptionHandler(ILogger<AppExceptionHandler> logger, TimeProvider timeProvider)
        {
            _logger = logger;
            _timeProvider = timeProvider;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An exception occured at {now}.", _timeProvider.GetUtcNow().UtcDateTime);
            var response = Map(exception);
            response.Extensions = new Dictionary<string, object?>()
            {
                { "traceId", httpContext.TraceIdentifier }
            };
            if (response is ValidationProblemDetails validationProblemDetailsResponse)
            {
                httpContext.Response.StatusCode = validationProblemDetailsResponse.Status ?? throw new ArgumentNullException();
                await httpContext.Response.WriteAsJsonAsync(validationProblemDetailsResponse, cancellationToken);
                return true;
            }
            httpContext.Response.StatusCode = (int)response.Status!;
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
            return true;
        }
        private ProblemDetails Map(Exception exception)
            => exception switch
            {
                ValidationException e => new ValidationProblemDetails()
                {
                    Type = _badRequestTypeUrl,
                    Title = "An validation exception occured.",
                    Status = (int)HttpStatusCode.BadRequest,
                    Errors = e.Errors,
                    Detail = e.Message
                },
                LinksyException e => new ProblemDetails()
                {
                    Type = _badRequestTypeUrl,
                    Title = "An exception occured.",
                    Status = (int)HttpStatusCode.BadRequest,
                    Detail = e.Message,
                },
                Exception e => new ProblemDetails()
                {
                    Type = _serverErrorTypeUrl,
                    Title = "There was a server error",
                    Status = (int)HttpStatusCode.InternalServerError,
                    Detail = "Ther was an internal server error. Please try again later.",
                }
            };
    }
}
