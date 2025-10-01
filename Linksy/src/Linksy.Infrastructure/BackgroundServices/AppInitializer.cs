using Linksy.Application.Shared.Configuration;
using Linksy.Domain.Entities;
using Linksy.Domain.Enums;
using Linksy.Infrastructure.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.BackgroundServices
{
    internal class AppInitializer : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AppInitializer> _logger;

        public AppInitializer(IServiceProvider serviceProvider, ILogger<AppInitializer> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting Linksy...");
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<LinksyDbContext>();
            if (dbContext.Database.GetPendingMigrations().Any())
            {
                await dbContext.Database.MigrateAsync(cancellationToken);
            }
            await CreateTestUser();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping Linksy...");
            return Task.CompletedTask;
        }

        private async Task CreateTestUser()
        {
            using var scope = _serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var linksyConfig = _serviceProvider.GetRequiredService<LinksyConfig>();
            var testUserConfig = linksyConfig.TestUser;
            var isUserCreated = await userManager.FindByEmailAsync(testUserConfig.Email) is not null;
            if (isUserCreated)
            {
                return;
            }
            var user = new User(testUserConfig.Email, testUserConfig.Username, testUserConfig.FirstName, testUserConfig.LastName, Gender.Undefined);
            await DoUserManagerActionAsync(async () => await userManager.CreateAsync(user, testUserConfig.Password));
            user = await userManager.FindByEmailAsync(testUserConfig.Email);
            if (user is null)
            {
                throw new NullReferenceException("User is null.");
            }
            await DoUserManagerActionAsync(async () => await userManager.AddToRoleAsync(user, "User"));
            await DoUserManagerActionAsync(async () =>
            {
                return await userManager.AddClaimsAsync(user, new List<Claim>()
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Email, user.Email!),
                    new(ClaimTypes.Name, user.UserName!)
                });
            });
            user.EmailConfirmed = true;
            await userManager.UpdateAsync(user);
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
    }
}
