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
    internal class QrCodeEngagementConfiguration : IEntityTypeConfiguration<QrCodeEngagement>
    {
        public void Configure(EntityTypeBuilder<QrCodeEngagement> builder)
        {
            builder.ToTable("QrCodeEngagements");   
            builder.HasOne(q => q.QrCode)
                .WithMany(q => q.Engagements)
                .HasForeignKey(q => q.QrCodeId);
        }
    }
}
