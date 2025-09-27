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
    internal class QrCodeConfiguration : IEntityTypeConfiguration<QrCode>
    {
        public void Configure(EntityTypeBuilder<QrCode> builder)
        {
            builder.Property(q => q.ScanCount)
                .IsRequired();
            builder.Property(q => q.ImageUrlPath)
                .IsRequired();
            builder.Property(q => q.IsUmtOn)
                .IsRequired(); 
        }
    }
}
