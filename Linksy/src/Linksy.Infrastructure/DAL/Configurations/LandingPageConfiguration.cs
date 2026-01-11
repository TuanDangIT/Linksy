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
            builder.ToTable("LandingPages", t =>
            {
                t.HasCheckConstraint("CK_LandingPage_ViewCount", "\"ViewCount\" >= 0");
                t.HasCheckConstraint("CK_LandingPage_EngagementCount", "\"EngagementCount\" >= 0");
            });
            builder.Property(l => l.Title)
                .HasMaxLength(128)
                .IsRequired();
            builder.Property(l => l.Description)
                .HasMaxLength(512);
            builder.Property(l => l.Code)
                .HasMaxLength(128)
                .IsRequired();
            builder.HasIndex(l => l.Code)
                .IsUnique();
            builder.Property(l => l.IsPublished)
                .IsRequired();
            builder.Property(l => l.EngagementCount)
                .IsRequired();
            builder.Property(l => l.ViewCount)
                .IsRequired();
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
            builder.OwnsOne(l => l.BackgroundImage, b =>
            {
                b.Property(bi => bi.FileName)
                    .HasMaxLength(256);
                b.Property(bi => bi.UrlPath)
                    .HasMaxLength(512);
            });
            builder.OwnsOne(l => l.LogoImage, b =>
            {
                b.Property(li => li.FileName)
                    .HasMaxLength(256);
                b.Property(li => li.UrlPath)
                    .HasMaxLength(512);
            });
            builder.Property(l => l.UserId)
                .IsRequired();
        }
    }
}
