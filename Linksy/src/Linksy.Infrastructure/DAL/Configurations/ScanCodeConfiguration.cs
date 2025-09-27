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
    internal class ScanCodeConfiguration : IEntityTypeConfiguration<ScanCode>
    {
        public void Configure(EntityTypeBuilder<ScanCode> builder)
        {
            builder.HasOne(s => s.Url)
                .WithOne(u => u.ScanCode)
                .HasForeignKey<ScanCode>(s => s.UrlId);
            builder.Property(s => s.Tags)
                .HasConversion(
                    v => v != null ? string.Join(',', v) : string.Empty,
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
            builder.ToTable(s =>
            {
                s.HasCheckConstraint("CK_ScanCode_ScanCount", "\"ScanCount\" >= 0");
            });
            builder.HasDiscriminator()
                .HasValue<QrCode>(nameof(QrCode))
                .HasValue<Barcode>(nameof(Barcode));    
        }
    }
}
