using Linksy.Application.Abstractions;
using Linksy.Application.Shared.Auth;
using Linksy.Application.Users.DTO;
using Linksy.Domain.Entities;
using Linksy.Domain.Enums;
using Linksy.Infrastructure.DAL;
using Linksy.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Services
{
    internal class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthManager _authManager;
        private readonly ILogger<IdentityService> _logger;
        private readonly RoleManager<Role> _roleManager;
        private readonly TimeProvider _timeProvider;
        private readonly IContextService _contextService;
        private readonly string _passwordPattern = @"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{6,}$";

        public IdentityService(UserManager<User> userManager, IAuthManager authManager, ILogger<IdentityService> logger,
            RoleManager<Role> roleManager, TimeProvider timeProvider, IContextService contextService)
        {
            _userManager = userManager;
            _authManager = authManager;
            _logger = logger;
            _roleManager = roleManager;
            _timeProvider = timeProvider;
            _contextService = contextService;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            if(dto.Password != dto.ConfirmPassword)
            {
                throw new PasswordNotMatchConfirmPasswordException();
            }
            var passwordRegex = new Regex(_passwordPattern);
            if (!passwordRegex.IsMatch(dto.Password))
            {
                throw new WeakPasswordException();
            }
            if (!Enum.TryParse(typeof(Gender), dto.Gender, out var gender))
            {
                throw new NotSupportedException();
            }
            if (await _userManager.FindByEmailAsync(dto.Email) is not null)
            {
                throw new EmailAlreadyExistsException(dto.Email);
            }
            if (await _userManager.FindByNameAsync(dto.Username) is not null)
            {
                throw new UsernameAlreadyExistsException(dto.Username);
            }
            var user = new User(dto.Email, dto.Username, dto.FirstName, dto.LastName, (Gender)gender);
            await DoUserManagerActionAsync(async () => await _userManager.CreateAsync(user, dto.Password));
            user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
            {
                throw new NullReferenceException("User is null.");
            }
            await DoUserManagerActionAsync(async () => await _userManager.AddToRoleAsync(user, "User"));
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            _logger.LogInformation("User with email {email} has registered.", dto.Email);
            return code;
        }

        public async Task<JwtDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email) ??
                throw new InvalidCredentialsException("Invalid email or password.");
            var isConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (!isConfirmed)
            {
                throw new UserNotConfirmedException();
            }
            var isLockedOut = await _userManager.IsLockedOutAsync(user);
            if (isLockedOut)
            {
                throw new UserLockedOutException();
            }
            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!isPasswordCorrect)
            {
                await _userManager.AccessFailedAsync(user);
                throw new InvalidCredentialsException("Invalid email or password.");
            }

            await DoUserManagerActionAsync(async () => await _userManager.ResetAccessFailedCountAsync(user));
            var roles = await _userManager.GetRolesAsync(user);
            var token = _authManager.CreateJwt(user.Id, user.UserName!, user.Email!, [.. roles]);
            var hashedRefreshToken = HashToken(token.RefreshToken);
            user.SetRefreshToken(hashedRefreshToken, token.RefreshTokenExpiryDate);
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                throw new DbUpdateException("User was not updated.");
            }
            _logger.LogInformation("Successfully created JWT Token: {@token} for user with email {email}.", token, user.Email);
            return token;

        }

        public async Task<JwtDto> RefreshTokenAsync(GetRefreshTokenDto dto)
        {
            var principal = _authManager.GetPrincipalFromExpiredToken(dto.JwtToken);
            var email = principal.FindFirst(c => c.Type == ClaimTypes.Email)?.Value
                ?? throw new InvalidCredentialException("Given JWT token is invalid.");
            var user = await _userManager.FindByEmailAsync(email);
            var hashedInputRefreshToken = HashToken(dto.RefreshToken);
            var now = _timeProvider.GetUtcNow();
            if(user is null || user.RefreshToken != hashedInputRefreshToken || user.RefreshTokenExpiryDate <= now)
            {
                throw new InvalidCredentialException("Invalid refresh token.");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var newJwtToken = _authManager.CreateJwt(user.Id, user.UserName!, user.Email!, [.. roles]);
            var hashedNewRefreshToken = HashToken(newJwtToken.RefreshToken);
            user.SetRefreshToken(hashedNewRefreshToken, newJwtToken.RefreshTokenExpiryDate);
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                throw new DbUpdateException("User was not updated.");
            }
            _logger.LogInformation("Successfully refreshed JWT Token: {@newJwtToken} for user with email {email}.", newJwtToken, user.Email);
            return newJwtToken;
        }

        public async Task ConfirmEmailAsync(ConfirmEmailDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email) ?? throw new InvalidEmailException(dto.Email);
            await DoUserManagerActionAsync(async () => await _userManager.ConfirmEmailAsync(user, dto.Code));
            _logger.LogInformation("User with email {email} has confirmed their email.", dto.Email);
        }

        public async Task<string> GetResetPasswordTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email) ?? throw new InvalidEmailException(email);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            _logger.LogInformation("User with email {email} has requested a password reset.", email);
            return token;
        }

        public async Task ResetPasswordAsync(ResetPasswordDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
            {
                throw new PasswordNotMatchConfirmPasswordException();
            }
            var user = await _userManager.FindByEmailAsync(dto.Email) ?? throw new InvalidEmailException(dto.Email);
            await DoUserManagerActionAsync(async () => await _userManager.ResetPasswordAsync(user, dto.Token, dto.Password));
            _logger.LogInformation("User with email {email} has resetted password.", user.Email);
        }

        private async Task DoUserManagerActionAsync(Func<Task<IdentityResult>> action)
        {
            var identityResult = await action();
            if (!identityResult.Succeeded)
            {
                var errorMessages = JsonSerializer.Serialize(identityResult.Errors);
                throw new InvalidOperationException(errorMessages);
            }
        }
        private string HashToken(string token)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
                return Convert.ToBase64String(bytes);
            }
        }

        public async Task<UserDto?> GetUserAsync()
        {
            var userId = _contextService.Identity!.Id;
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if(user is null) return null;
            return new UserDto(user.Id, user.UserName!, user.Email!, user.FirstName, user.LastName, user.Gender.ToString(), user.CreatedAt, user.UpdatedAt);
        }
    }
}
