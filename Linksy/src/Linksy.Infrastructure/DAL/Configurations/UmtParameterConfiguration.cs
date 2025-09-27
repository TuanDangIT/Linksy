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
    internal class UmtParameterConfiguration : IEntityTypeConfiguration<UmtParameter>
    {
        public void Configure(EntityTypeBuilder<UmtParameter> builder)
        {
            builder.Property(u => u.Type)
                .HasConversion<string>()
                .IsRequired();
            builder.Property(u => u.VisitCount)
                .IsRequired();
            builder.HasOne(u => u.QrCode)
                .WithMany(q => q.UmtParameters)
                .HasForeignKey(u => u.QrCodeId);
            builder.ToTable(u =>
            {
                u.HasCheckConstraint("CK_UmtParameter_VisitCount", "\"VisitCount\" >= 0");
            });
        }
    }
}
