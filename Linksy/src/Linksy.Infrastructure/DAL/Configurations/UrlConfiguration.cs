using Linksy.Domain.Entities.ScanCode;
using Linksy.Domain.Entities.Url;
using Linksy.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
            builder.ToTable("Urls");
            builder.Property(u => u.OriginalUrl)
                .HasMaxLength(512)
                .IsRequired();
            builder.Property(u => u.Code)
                .HasMaxLength(128)
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
            builder.Property(u => u.CreatedAt)
                .IsRequired();  
            builder.HasOne(u => u.QrCode)
                .WithOne(q => q.Url)
                .HasForeignKey<QrCode>(q => q.UrlId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(u => u.Barcode)
                .WithOne(b => b.Url)
                .HasForeignKey<Barcode>(b => b.UrlId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Ignore(u => u.TagsList);
            //builder.Property(u => u.Tags)
            //    .HasConversion(
            //        v => v != null ? string.Join(',', v) : string.Empty,
            //        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
            //        new ValueComparer<IEnumerable<string>>(
            //            (c1, c2) => c1!.SequenceEqual(c2!),
            //            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            //            c => c.ToList()));
        }
    }
}
