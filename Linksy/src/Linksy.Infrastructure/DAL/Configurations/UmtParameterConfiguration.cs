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
            builder.HasOne(u => u.Url)
                .WithMany(u => u.UmtParameters)
                .HasForeignKey(u => u.UrlId);
            builder.HasOne(u => u.QrCode)
                .WithMany()
                .HasForeignKey(u => u.QrCodeId);
            builder.Property(u => u.VisitCount)
                .IsRequired();
            builder.Property(u => u.UmtSource)
                .HasMaxLength(128);
            builder.Property(u => u.UmtMedium)
                .HasMaxLength(128);
            builder.Property(u => u.UmtCampaign)
                .HasMaxLength(128);
            builder.ToTable(u =>
            {
                u.HasCheckConstraint("CK_UmtParameter_VisitCount", "\"VisitCount\" >= 0");
            });
        }
    }
}
