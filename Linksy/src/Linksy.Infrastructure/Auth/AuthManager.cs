using Linksy.Application.Shared.Auth;
using Linksy.Application.Users.DTO;
using Linksy.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Auth
{
    internal class AuthManager : IAuthManager
    {
        private readonly AuthOptions _authOptions;
        private readonly TimeProvider _timeProvider;
        private readonly SymmetricSecurityKey _securityKey;

        public AuthManager(AuthOptions authOptions, TimeProvider timeProvider)
        {
            _authOptions = authOptions;
            _timeProvider = timeProvider;
            _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.SigningKey));
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }
        public JwtDto CreateJwt(int userId, string username, string email, List<string> roles)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userId.ToString()),
                new(ClaimTypes.Email, email),
                new(ClaimTypes.Name, username)
            };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
            var expiryDate = _timeProvider.GetUtcNow().LocalDateTime.AddMinutes(_authOptions.ExpiryInMinutes);
            var jwt = new JwtSecurityToken(
                issuer: _authOptions.Issuer,
                audience: _authOptions.Audience,
                claims: claims,
                expires: expiryDate,
                signingCredentials: new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256)
            );
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(jwt);
            var refreshToken = CreateRefreshToken();
            return new JwtDto()
            {
                JwtToken = token,
                JwtTokenExpiryDate = expiryDate,
                JwtTokenExpiryInMinutes = _authOptions.ExpiryInMinutes,
                RefreshToken = refreshToken.RefreshToken,
                RefreshTokenExpiryDate = refreshToken.ExpiryDate,
                RefreshTokenExpiryInDays = _authOptions.RefreshTokenExpiryInDays,
            };
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = _authOptions.Issuer,
                ValidAudience = _authOptions.Audience,
                ValidateAudience = _authOptions.ValidateAudience,
                ValidateIssuer = _authOptions.ValidateIssuer,
                ValidateIssuerSigningKey = _authOptions.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.SigningKey)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || 
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
                throw new InvalidCredentialsException("Invalid token.");
            return principal;
        }

        public RefreshTokenDto CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                var refreshToken = Convert.ToBase64String(randomNumber);
                var expiryDate = _timeProvider.GetUtcNow().UtcDateTime.AddDays(_authOptions.RefreshTokenExpiryInDays);
                return new RefreshTokenDto()
                {
                    RefreshToken = refreshToken,
                    ExpiryDate = expiryDate
                };
            }
        }
    }
}
