using FluentValidation;
using Linksy.Application.Shared.Behaviors;
using Linksy.Application.Shared.ScanCodes;
using Linksy.Application.Urls.Features.ShortenUrl;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            services.Scan(i => i.FromAssemblies(Assembly.GetExecutingAssembly())
                .AddClasses(c => c.AssignableTo(typeof(IValidator<>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime());
            services.AddMediatR(cfg =>
            {
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            services.AddSingleton(TimeProvider.System);
            return services;
        }
    }
}
