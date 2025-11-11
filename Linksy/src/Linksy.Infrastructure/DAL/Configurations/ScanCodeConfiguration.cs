using Linksy.Domain.Entities.ScanCode;
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
            //builder.Property(s => s.Tags)
            //    .HasConversion(
            //        v => v != null ? string.Join(',', v) : string.Empty,
            //        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
            //        new ValueComparer<IEnumerable<string>>(
            //            (c1, c2) => c1!.SequenceEqual(c2!),
            //            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            //            c => c.ToList()));
            builder.Property(s => s.CreatedAt)
                .IsRequired();
            builder.Property(s => s.ScanCount)
                .IsRequired();
            builder.Property(s => s.IsActive)
                .IsRequired();
            builder.Property(s => s.ImageUrlPath)
                .HasMaxLength(512);
            builder.Ignore(s => s.TagsList);
        }
    }
}
