using Linksy.Domain.Entities;
using Linksy.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.DAL.Configurations
{
    internal class UrlConfiguration : IEntityTypeConfiguration<Url>
    {
        public void Configure(EntityTypeBuilder<Url> builder)
        {
            builder.Property(u => u.OriginalUrl)
                .IsRequired();
            builder.Property(u => u.Code)
                .IsRequired();
            builder.HasIndex(u => u.Code)
                .IsUnique();
            builder.Property(u => u.IsActive)
                .IsRequired();
            builder.Property(u => u.VisitCount)
                .IsRequired();
            builder.ToTable(u =>
            {
                u.HasCheckConstraint("CK_Url_VisitCount", "\"VisitCount\" >= 0");
            });
            builder.HasOne(u => u.QrCode)
                .WithOne(q => q.Url)
                .HasForeignKey<QrCode>(q => q.UrlId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(u => u.Barcode)
                .WithOne(b => b.Url)
                .HasForeignKey<Barcode>(b => b.UrlId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
