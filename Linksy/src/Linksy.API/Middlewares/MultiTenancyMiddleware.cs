using Linksy.Infrastructure.Services;
using System.Security.Claims;

namespace Linksy.API.Middlewares
{
    public class MultiTenancyMiddleware : IMiddleware
    {
        private readonly IMultiTenancyService _multiTenancyService;
        private readonly ILogger<MultiTenancyMiddleware> _logger;

        public MultiTenancyMiddleware(IMultiTenancyService multiTenancyService, ILogger<MultiTenancyMiddleware> logger)
        {
            _multiTenancyService = multiTenancyService;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var user = context.User;
            if (user is null)
            {
                await next(context);
            }
            if (user!.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
            {
                var id = int.Parse(user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
                _multiTenancyService.SetCurrentTenant(id);
                _logger.LogDebug("Tenant was set to {tenantId}.", id);
            }
            await next(context);
        }
    }
}
