using Linksy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
            builder.UseTpcMappingStrategy();
            builder.Property(s => s.Tags)
                .HasConversion(
                    v => v != null ? string.Join(',', v) : string.Empty,
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
                    new ValueComparer<List<string>>(
                        (c1, c2) => c1!.SequenceEqual(c2!),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.ToList()));
            builder.HasDiscriminator()
                .HasValue<QrCode>(nameof(QrCode))
                .HasValue<Barcode>(nameof(Barcode));    
        }
    }
}
