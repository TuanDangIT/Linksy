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
            builder.Property(s => s.CreatedAt)
                .IsRequired();
            builder.Property(s => s.ScanCount)
                .IsRequired();
            builder.Ignore(s => s.TagsList);
            builder.Property(s => s.UserId)
                .IsRequired();
        }
    }
}
