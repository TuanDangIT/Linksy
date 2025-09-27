using Linksy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.DAL.Configurations
{
    internal class LandingPageItemConfiguration : IEntityTypeConfiguration<LandingPageItem>
    {
        public void Configure(EntityTypeBuilder<LandingPageItem> builder)
        {
            builder.Property(l => l.Content)
                .HasMaxLength(64)
                .IsRequired();
            builder.Property(l => l.BackgroundColor)
                .HasMaxLength(16)
                .IsRequired();
            builder.Property(l => l.FontColor)
                .HasMaxLength(16)
                .IsRequired();
            builder.Property(l => l.Order)
                .IsRequired();
            builder.HasOne(l => l.Url)
                .WithOne(u => u.LandingPageItem)
                .HasForeignKey<LandingPageItem>(l => l.UrlId);
            builder.HasOne(l => l.LandingPage)
                .WithMany(lp => lp.LandingPageItems)
                .HasForeignKey(l => l.LandingPageId);
        }
    }
}
