using Linksy.Domain.Entities;
using Linksy.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
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
            builder.ToTable("Url", u =>
            {
                u.HasCheckConstraint("CK_Url_VisitCount", "\"VisitCount\" >= 0");
            });
        }
    }
}
