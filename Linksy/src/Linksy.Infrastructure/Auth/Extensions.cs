using Linksy.Application.Shared.Auth;
using Linksy.Domain.Entities;
using Linksy.Infrastructure.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Auth
{
    public static class Extensions
    {
        private static readonly string _authSectionName = "Authentication";
        public static IServiceCollection AddAuth(this IServiceCollection services)
        {
            var authOptions = services.GetOptions<AuthOptions>(_authSectionName);
            services.AddSingleton(authOptions);
            services.AddSingleton<IAuthManager, AuthManager>();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Challenge = JwtBearerDefaults.AuthenticationScheme;
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authOptions.Issuer,
                    ValidAudience = authOptions.Audience,
                    ValidateAudience = authOptions.ValidateAudience,
                    ValidateIssuer = authOptions.ValidateIssuer,
                    ValidateIssuerSigningKey = authOptions.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.SigningKey)),
                    ValidateLifetime = authOptions.ValidateLifetime
                };
            });
            services.AddIdentityCore<User>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(20);
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
            })
                .AddRoles<Role>()
                .AddEntityFrameworkStores<LinksyDbContext>()
                .AddDefaultTokenProviders();
            services.AddAuthorization();
            return services;
        }
    }
}
