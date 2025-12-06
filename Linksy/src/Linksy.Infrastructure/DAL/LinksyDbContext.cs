using Linksy.Domain.Abstractions;
using Linksy.Domain.Entities.LandingPage;
using Linksy.Domain.Entities.ScanCode;
using Linksy.Domain.Entities.Tracking;
using Linksy.Domain.Entities.Url;
using Linksy.Domain.Entities.User;
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
        public DbSet<UmtParameter> UmtParameters { get; set; }
        public DbSet<QrCode> QrCodes { get; set; }
        public DbSet<Barcode> Barcodes { get; set; }
        public DbSet<LandingPage> LandingPages { get; set; }
        public DbSet<LandingPageItem> LandingPageItems { get; set; }
        public DbSet<BarcodeEngagement> BarcodeEngagements { get; set; }
        public DbSet<QrCodeEngagement> QrCodeEngagements { get; set; }
        public DbSet<LandingPageEngagement> LandingPageEngagements { get; set; }
        public DbSet<LandingPageView> LandingPageViews { get; set; }
        public DbSet<UmtParameterEngagement> UmtParameterEngagements { get; set; }
        public DbSet<UrlEngagement> UrlEngagements { get; set; }
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
            builder.Entity<UmtParameter>().HasQueryFilter(u => u.UserId == _multiTenancyService.CurrentTenantId);
            builder.Entity<LandingPageItem>().HasQueryFilter(l => l.UserId == _multiTenancyService.CurrentTenantId);
            base.OnModelCreating(builder);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var auditableEntries = ChangeTracker.Entries<IAuditable>();
            var now = _timeProvider.GetUtcNow().UtcDateTime;
            foreach (var entry in auditableEntries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(nameof(IAuditable.CreatedAt)).CurrentValue = now;
                }
                if (entry.State == EntityState.Modified)
                {
                    entry.Property(nameof(IAuditable.UpdatedAt)).CurrentValue = now;
                }
            }

            var hasEngagementTimeEntries = ChangeTracker.Entries<IHasEngagementTime>();
            foreach (var entry in hasEngagementTimeEntries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(nameof(IHasEngagementTime.EngagedAt)).CurrentValue = now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
