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
        Task<JwtDto> LoginAsync(LoginDto dto);
        Task<JwtDto> RefreshTokenAsync(GetRefreshTokenDto dto);
        Task DeleteRefreshTokenAsync();
        Task ConfirmEmailAsync(ConfirmEmailDto dto);
        Task<string> GetResetPasswordTokenAsync(string email);
        Task ResetPasswordAsync(ResetPasswordDto dto);
    }
}
