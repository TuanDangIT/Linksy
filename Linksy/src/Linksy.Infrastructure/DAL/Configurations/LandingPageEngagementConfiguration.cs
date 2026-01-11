using Linksy.Domain.Entities.Tracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.DAL.Configurations
{
    internal class LandingPageEngagementConfiguration : IEntityTypeConfiguration<LandingPageEngagement>
    {
        public void Configure(EntityTypeBuilder<LandingPageEngagement> builder)
        {
            builder.ToTable("LandingPageEngagements");
            builder.HasOne(lpe => lpe.LandingPage)
                .WithMany(lp => lp.Engagements)
                .HasForeignKey(lpe => lpe.LandingPageId);
        }
    }
}
