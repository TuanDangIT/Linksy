using Linksy.Application.Users.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Services
{
    public interface IIdentityService
    {
        Task<UserDto> GetUserAsync();
        Task<string> RegisterAsync(RegisterDto dto);
        Task<UserWithJwtDto> LoginAsync(LoginDto dto);
        Task LogoutAsync();
        Task<JwtDto> RefreshTokenAsync(GetRefreshTokenDto dto);
        Task ConfirmEmailAsync(ConfirmEmailDto dto);
        Task<string> GetResetPasswordTokenAsync(string email);
        Task ResetPasswordAsync(ResetPasswordDto dto);
    }
}
