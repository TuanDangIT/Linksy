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
    internal class UrlConfiguration : IEntityTypeConfiguration<Url>
    {
        public void Configure(EntityTypeBuilder<Url> builder)
        {
            builder.ToTable("Url");
            builder.Property(u => u.OriginalUrl)
                .IsRequired();
            builder.Property(u => u.Code)
                .IsRequired();
            builder.HasIndex(u => u.Code)
                .IsUnique();
            builder.Property(u => u.IsActive)
                .IsRequired();
            builder.Property(u => u.VisitCount)
                .IsRequired();
            builder.ToTable(u =>
            {
                u.HasCheckConstraint("CK_Url_VisitCount", "\"VisitCount\" >= 0");
            });
        }
    }
}
