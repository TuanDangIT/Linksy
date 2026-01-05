using Azure;
using Linksy.Application.Users.DTO;

namespace Linksy.API.Utils
{
    public static class CookieUtils
    {
        public static void SetTokenCookies(HttpContext context, JwtDto token)
        {
            var fiveMinutes = TimeSpan.FromMinutes(5);
            var refreshTokenExpiration = TimeSpan.FromDays(token.RefreshTokenExpiryInDays) + fiveMinutes;
            var isProduction = context.RequestServices.GetRequiredService<IWebHostEnvironment>().IsProduction();
            Console.WriteLine($"Setting token cookies. {token.JwtToken} ::::::: {token.RefreshToken} ::::::::::::::::::::::::::");

            context.Response.Cookies.Append("jwtToken", token.JwtToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = isProduction,
                SameSite = SameSiteMode.Lax,
                MaxAge = refreshTokenExpiration
            });

            context.Response.Cookies.Append("refreshToken", token.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = isProduction,
                SameSite = SameSiteMode.Lax,
                MaxAge = refreshTokenExpiration
            });
            Console.WriteLine($"APPENDED COOKIES:::::::::::::::::::::::::::::: {token.RefreshToken}");
        }

        public static void DeleteTokenCookies(HttpContext context)
        {
            context.Response.Cookies.Delete("jwtToken");
            context.Response.Cookies.Delete("refreshToken");
        }
    }
}
