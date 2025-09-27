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
    internal class QrCodePageConfiguration : IEntityTypeConfiguration<QrCodePage>
    {
        public void Configure(EntityTypeBuilder<QrCodePage> builder)
        {
            builder.Property(q => q.VisitCount)
                .IsRequired();
            builder.Property(q => q.Code)
                .IsRequired();
            builder.Property(q => q.BackgroundColor)
                .HasMaxLength(16)
                .IsRequired();
            builder.HasOne(q => q.QrCode)
                .WithOne(qr => qr.QrCodePage)
                .HasForeignKey<QrCodePage>(q => q.QrCodeId);
            builder.ToTable(q =>
            {
                q.HasCheckConstraint("CK_QrCodePage_VisitCount", "\"VisitCount\" >= 0");
            });
        }
    }
}
