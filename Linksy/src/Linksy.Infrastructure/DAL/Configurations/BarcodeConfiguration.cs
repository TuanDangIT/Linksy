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
    internal class BarcodeConfiguration : IEntityTypeConfiguration<Barcode>
    {
        public void Configure(EntityTypeBuilder<Barcode> builder)
        {
            builder.ToTable("Barcodes", s =>
            {
                s.HasCheckConstraint("CK_Barcode_ScanCount", "\"ScanCount\" >= 0");
            });
        }
    }
}
