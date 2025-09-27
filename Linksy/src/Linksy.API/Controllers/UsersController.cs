using Asp.Versioning;
using Linksy.Application.Users.DTO;
using Linksy.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Linksy.API.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IIdentityService _identityService;

        public UsersController(IIdentityService userService, IMediator mediator) : base(mediator)
        {
            _identityService = userService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<string>> Register([FromBody] RegisterDto dto)
        {
            var token = await _identityService.RegisterAsync(dto);
            return Ok(token);
        }
        [HttpPost("login")]
        public async Task<ActionResult<JwtDto>> Login([FromBody] LoginDto dto)
        {
            var token = await _identityService.LoginAsync(dto);
            return Ok(token);
        }
        [HttpPost("confirm-email")]
        public async Task<ActionResult> ConfirmEmail([FromBody] ConfirmEmailDto dto)
        {
            await _identityService.ConfirmEmailAsync(dto);
            return NoContent();
        }
        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword([FromBody] string email)
        {
            var token = await _identityService.GetResetPasswordTokenAsync(email);
            return Ok(token);
        }
        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            await _identityService.ResetPasswordAsync(dto);
            return NoContent();
        }
        [HttpPost("refresh-token")]
        public async Task<ActionResult<JwtDto>> RefreshToken([FromBody] GetRefreshTokenDto dto)
        {
            var token = await _identityService.RefreshTokenAsync(dto);
            return Ok(token);
        }
    }
}
