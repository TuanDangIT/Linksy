using Linksy.Domain.Abstractions;
using Linksy.Domain.Entities;
using Linksy.Infrastructure.Services;
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
        public DbSet<Url> Urls { get; set; }
        public DbSet<QrCode> QrCodes { get; set; }
        public DbSet<Barcode> Barcodes { get; set; }
        public DbSet<LandingPage> LandingPages { get; set; }
        public DbSet<LandingPageItem> LandingPageItems { get; set; }
        private readonly TimeProvider _timeProvider;
        private readonly IMultiTenancyService _multiTenancyService;

        public LinksyDbContext(DbContextOptions<LinksyDbContext> options, TimeProvider timeProvider, IMultiTenancyService multiTenancyService) : base(options)
        {
            _timeProvider = timeProvider;
            _multiTenancyService = multiTenancyService;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Entity<Url>().HasQueryFilter(u => u.UserId == _multiTenancyService.CurrentTenantId);
            builder.Entity<ScanCode>().HasQueryFilter(q => q.UserId == _multiTenancyService.CurrentTenantId);
            builder.Entity<LandingPage>().HasQueryFilter(l => l.UserId == _multiTenancyService.CurrentTenantId);
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
