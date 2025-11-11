using Linksy.Domain.Entities.ScanCode;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.DAL.Configurations
{
    internal class QrCodeConfiguration : IEntityTypeConfiguration<QrCode>
    {
        public void Configure(EntityTypeBuilder<QrCode> builder)
        {
            builder.ToTable("QrCodes", s =>
            {
                s.HasCheckConstraint("CK_QrCode_ScanCount", "\"ScanCount\" >= 0");
            });
            builder.HasOne(q => q.UmtParameter)
                .WithOne(u => u.QrCode)
                .HasForeignKey<QrCode>(u => u.UmtParameterId);
        }
    }
}
