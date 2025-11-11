using Linksy.Domain.Entities.LandingPage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.DAL.Configurations
{
    internal class LandingPageConfiguration : IEntityTypeConfiguration<LandingPage>
    {
        public void Configure(EntityTypeBuilder<LandingPage> builder)
        {
            builder.ToTable("LaningPages", t =>
            {
                t.HasCheckConstraint("CK_LandingPage_ViewCount", "\"ViewCount\" >= 0");
                t.HasCheckConstraint("CK_LandingPage_EngagementCount", "\"EngagementCount\" >= 0");
            });
            builder.Property(l => l.Title)
                .HasMaxLength(128)
                .IsRequired();
            builder.Property(l => l.Description)
                .HasMaxLength(512);
            builder.Property(l => l.BackgroundColor)
                .HasMaxLength(64);
            builder.Property(l => l.Code)
                .HasMaxLength(128)
                .IsRequired();
            builder.HasIndex(l => l.Code)
                .IsUnique();
            builder.Property(l => l.IsActive)
                .IsRequired();
            builder.Property(l => l.EngagementCount)
                .IsRequired();
            builder.Property(l => l.ViewCount)
                .IsRequired();
            builder.Property(l => l.BackgroundImageUrl)
                .HasMaxLength(512);
            builder.Property(l => l.ImageUrlPath)
                .HasMaxLength(512);
            builder.Property(l => l.Description)
                .HasMaxLength(1024);
            builder.Property(l => l.BackgroundColor)
                .HasMaxLength(16);
            builder.Property(l => l.CreatedAt)
                .IsRequired();
            builder.Property(l => l.TitleFontColor)
                .HasMaxLength(16)
                .IsRequired();
            builder.Property(l => l.DescriptionFontColor)
                .HasMaxLength(16);
            builder.Ignore(l => l.TagsList);
        }
    }
}
