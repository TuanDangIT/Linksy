using Linksy.Domain.Abstractions;
using Linksy.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.DAL
{
    internal class LinksyDbContext : IdentityDbContext<User, Role, int>
    {
        private readonly TimeProvider _timeProvider;
        public LinksyDbContext(DbContextOptions<LinksyDbContext> options, TimeProvider timeProvider) : base(options)
        {
            _timeProvider = timeProvider;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<IAuditable>();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(nameof(IAuditable.CreatedAt)).CurrentValue = _timeProvider.GetUtcNow().UtcDateTime;
                }
                if (entry.State == EntityState.Modified)
                {
                    entry.Property(nameof(IAuditable.UpdatedAt)).CurrentValue = _timeProvider.GetUtcNow().UtcDateTime;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
