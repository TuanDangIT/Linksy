using Azure.Storage.Blobs.Models;
using Linksy.Domain.Entities.LandingPage;
using Linksy.Domain.Enums;
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
            builder.ToTable("LandingPageItems", l =>
            {
                l.HasCheckConstraint("CK_LandingPageItem_ClickCount", "\"ClickCount\" >= 0");
                l.HasCheckConstraint("CK_LandingPageItem_Order", "\"Order\" >= 0");
            });
            builder.Property(l => l.Type)
                .HasConversion<string>()
                .IsRequired();
            builder.Property(l => l.Order)
                .IsRequired();
            builder.Property(l => l.CreatedAt)
                .IsRequired();
            builder.Property(l => l.ClickCount)
                .IsRequired();
            builder.HasOne(l => l.LandingPage)
                .WithMany(lp => lp.LandingPageItems)
                .HasForeignKey(l => l.LandingPageId);
            builder.HasDiscriminator(l => l.Type)
                .HasValue<ImageLandingPageItem>(LandingPageItemType.Image)
                .HasValue<TextLandingPageItem>(LandingPageItemType.Text)
                .HasValue<UrlLandingPageItem>(LandingPageItemType.Url)
                .HasValue<YouTubeLandingPageItem>(LandingPageItemType.YouTube);
        }
    }

    internal class YoutubeLandingPageItemConfiguration : IEntityTypeConfiguration<YouTubeLandingPageItem>
    {
        public void Configure(EntityTypeBuilder<YouTubeLandingPageItem> builder)
        {
            //builder.ToTable("YoutubeLandingPageItems");
            builder.Property(y => y.VideoUrl)
                .HasMaxLength(512)
                .IsRequired();
        }
    }

    internal class UrlLandingPageItemConfiguration : IEntityTypeConfiguration<UrlLandingPageItem>
    {
        public void Configure(EntityTypeBuilder<UrlLandingPageItem> builder)
        {
            //builder.ToTable("UrlLandingPageItems");
            builder.Property(u => u.Content)
                .HasMaxLength(128)
                .IsRequired();
            builder.Property(u => u.BackgroundColor)
                .HasMaxLength(16)
                .IsRequired();
            builder.Property(u => u.FontColor)
                .HasMaxLength(16)
                .IsRequired();
            builder.HasOne(u => u.Url)
                .WithMany(u => u.UrlLandingPageItems)
                .HasForeignKey(u => u.UrlId);
        }
    }

    internal class TextLandingPageItemConfiguration : IEntityTypeConfiguration<TextLandingPageItem>
    {
        public void Configure(EntityTypeBuilder<TextLandingPageItem> builder)
        {
            //builder.ToTable("TextLandingPageItems");
            builder.Property(t => t.Content)
                .HasMaxLength(1024)
                .IsRequired();
            builder.Property(t => t.FontColor)
                .HasMaxLength(16)
                .IsRequired();
        }
    }

    internal class ImageLandingPageItemConfiguration : IEntityTypeConfiguration<ImageLandingPageItem>
    {
        public void Configure(EntityTypeBuilder<ImageLandingPageItem> builder)
        {
            //builder.ToTable("ImageLandingPageItems");
            builder.Property(i => i.ImageUrlPath)
                .HasMaxLength(512)
                .IsRequired();
            builder.Property(i => i.AltText)
                .HasMaxLength(256);
            builder.HasOne(i => i.Url)
                .WithMany(u => u.ImageLandingPageItems)
                .HasForeignKey(i => i.UrlId);
        }
    }
}
