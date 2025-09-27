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
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.SigningKey)),
                    ValidateLifetime = authOptions.ValidateLifetime
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            services.AddIdentityCore<User>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(20);
            })
                .AddRoles<Role>()
                .AddEntityFrameworkStores<LinksyDbContext>()
                .AddDefaultTokenProviders();
            services.AddAuthorization();
            return services;
        }
    }
}
