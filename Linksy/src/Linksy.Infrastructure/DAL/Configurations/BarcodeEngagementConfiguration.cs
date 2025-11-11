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
    internal class BarcodeEngagementConfiguration : IEntityTypeConfiguration<BarcodeEngagement>
    {
        public void Configure(EntityTypeBuilder<BarcodeEngagement> builder)
        {
            builder.ToTable("BarcodeEngagements");
            builder.HasOne(b => b.Barcode)
                .WithMany(b => b.Engagements)
                .HasForeignKey(b => b.BarcodeId);
        }
    }
}
