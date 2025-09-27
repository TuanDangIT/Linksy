using Linksy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.DAL.Configurations
{
    internal class LandingPageConfiguration : IEntityTypeConfiguration<LandingPage>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<LandingPage> builder)
        {
            builder.Property(builder => builder.Title)
                .HasMaxLength(64)
                .IsRequired();
            builder.Property(builder => builder.Description)
                .HasMaxLength(256);
            builder.Property(l => l.VisitCount)
                .IsRequired();
            builder.Property(l => l.Code)
                .IsRequired();
            builder.ToTable(l =>
            {
                l.HasCheckConstraint("CK_LandingPage_VisitCount", "\"VisitCount\" >= 0");
            });
        }
    }
}
