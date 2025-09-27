using Linksy.Application.Users.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Shared.Auth
{
    public interface IAuthManager
    {
        JwtDto CreateJwt(int userId, string username, string email, List<string> roles);
        RefreshTokenDto CreateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
