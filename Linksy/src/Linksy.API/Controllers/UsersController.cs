using Asp.Versioning;
using Linksy.API.API;
using Linksy.API.Utils;
using Linksy.Application.Users.DTO;
using Linksy.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security;

namespace Linksy.API.Controllers
{
    [ApiController]
    [Route("api/v{v:apiVersion}" + "/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public UsersController(IIdentityService userService) 
        {
            _identityService = userService;
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetUser()
        {
            var user = await _identityService.GetUserAsync(); /*?? throw new SecurityException("User's token is invalid.");*/
            return Ok(new ApiResponse<UserDto>(HttpStatusCode.OK, user));
        }
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<string>>> Register([FromBody] RegisterDto dto)
        {
            var emailConfirmationCode = await _identityService.RegisterAsync(dto);
            return Ok(new ApiResponse<string>(HttpStatusCode.OK, emailConfirmationCode));
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto dto)
        {
            var userWithJwt = await _identityService.LoginAsync(dto);
            CookieUtils.SetTokenCookies(HttpContext, userWithJwt.Jwt);
            return Ok(userWithJwt.User);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<string>>> Logout()
        {
            await _identityService.LogoutAsync();

            CookieUtils.DeleteTokenCookies(HttpContext);

            return Ok(new ApiResponse<string>(HttpStatusCode.OK, "Logged out successfully"));
        }
        //[HttpPost("confirm-email")]
        //public async Task<ActionResult> ConfirmEmail([FromBody] ConfirmEmailDto dto)
        //{
        //    await _identityService.ConfirmEmailAsync(dto);
        //    return NoContent();
        //}
        //[HttpPost("forgot-password")]
        //public async Task<ActionResult> ForgotPassword([FromBody] string email)
        //{
        //    var token = await _identityService.GetResetPasswordTokenAsync(email);
        //    return Ok(token);
        //}
        //[HttpPost("reset-password")]
        //public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        //{
        //    await _identityService.ResetPasswordAsync(dto);
        //    return NoContent();
        //}
        //[HttpPost("refresh-token")]
        //public async Task<ActionResult<JwtDto>> RefreshToken([FromBody] GetRefreshTokenDto dto)
        //{
        //    var token = await _identityService.RefreshTokenAsync(dto);
        //    SetTokenCookies(token);
        //    return Ok(token);
        //}

        //private void SetTokenCookies(JwtDto token)
        //{
        //    var accessTokenExpiration = TimeSpan.FromMinutes(token.JwtTokenExpiryInMinutes);
        //    var refreshTokenExpiration = TimeSpan.FromDays(token.RefreshTokenExpiryInDays);

        //    Response.Cookies.Append("jwtToken", token.JwtToken, new CookieOptions
        //    {
        //        HttpOnly = true, 
        //        Secure = true,   
        //        SameSite = SameSiteMode.Strict, 
        //        MaxAge = accessTokenExpiration
        //    });

        //    Response.Cookies.Append("refreshToken", token.RefreshToken, new CookieOptions
        //    {
        //        HttpOnly = true,
        //        Secure = true,
        //        SameSite = SameSiteMode.Strict,
        //        MaxAge = refreshTokenExpiration
        //    });
        //}
    }
}
