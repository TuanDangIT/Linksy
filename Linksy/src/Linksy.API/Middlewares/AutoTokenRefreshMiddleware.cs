using Linksy.API.Utils;
using Linksy.Application.Users.DTO;
using Linksy.Infrastructure.Auth;
using Linksy.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

public class AutoTokenRefreshMiddleware : IMiddleware
{
    private readonly IIdentityService _identityService;
    private readonly AuthOptions _authOptions;
    private readonly ILogger<AutoTokenRefreshMiddleware> _logger;

    public AutoTokenRefreshMiddleware(IIdentityService identityService, AuthOptions authOptions, ILogger<AutoTokenRefreshMiddleware> logger)
    {
        _identityService = identityService;
        _authOptions = authOptions;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var jwtToken = context.Request.Cookies["jwtToken"];
        var refreshToken = context.Request.Cookies["refreshToken"];

        if (!string.IsNullOrEmpty(jwtToken) && !string.IsNullOrEmpty(refreshToken))
        {
            try
            {
                await TryRefreshTokenIfNeededAsync(context, jwtToken, refreshToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during automatic token refresh");
            }
        }

        await next(context);
    }

    private async Task TryRefreshTokenIfNeededAsync(
        HttpContext context,
        string jwtToken,
        string refreshToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        if (!tokenHandler.CanReadToken(jwtToken))
        {
            return;
        }

        var token = tokenHandler.ReadJwtToken(jwtToken);
        var expirationTime = token.ValidTo;
        var timeUntilExpiry = expirationTime - DateTime.UtcNow;

        if (timeUntilExpiry.TotalMinutes < _authOptions.RefreshThresholdMinutes)
        {
            _logger.LogInformation(
                "Access token expiring in {Minutes} minutes, attempting to refresh token.",
                timeUntilExpiry.TotalMinutes);
            var newToken = await _identityService.RefreshTokenAsync(new GetRefreshTokenDto()
            {
                JwtToken = jwtToken,
                RefreshToken = refreshToken
            });
            CookieUtils.SetTokenCookies(context, newToken);
        }
    }

    //private void SetTokenCookies(HttpContext context, JwtDto token)
    //{
    //    var jwtTokenExpiration = TimeSpan.FromMinutes(token.JwtTokenExpiryInMinutes);
    //    var refreshTokenExpiration = TimeSpan.FromDays(token.RefreshTokenExpiryInDays);

    //    context.Response.Cookies.Append("accessToken", token.JwtToken, new CookieOptions
    //    {
    //        HttpOnly = true,
    //        Secure = true,
    //        SameSite = SameSiteMode.Strict,
    //        MaxAge = jwtTokenExpiration,
    //        Path = "/"
    //    });

    //    context.Response.Cookies.Append("refreshToken", token.RefreshToken, new CookieOptions
    //    {
    //        HttpOnly = true,
    //        Secure = true,
    //        SameSite = SameSiteMode.Strict,
    //        MaxAge = refreshTokenExpiration,
    //        Path = "/"
    //    });
    //}
}