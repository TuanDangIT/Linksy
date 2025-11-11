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
    internal class LandingPageViewConfiguration : IEntityTypeConfiguration<LandingPageView>
    {
        public void Configure(EntityTypeBuilder<LandingPageView> builder)
        {
            builder.ToTable("LandingPageViews");
            builder.Property(lpv => lpv.IpAddress)
                .HasMaxLength(64);
            builder.HasOne(lpv => lpv.LandingPage)
                .WithMany(lp => lp.Views)
                .HasForeignKey(lpv => lpv.LandingPageId);
        }
    }
}
