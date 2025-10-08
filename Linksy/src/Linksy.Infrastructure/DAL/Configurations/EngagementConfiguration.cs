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
    internal class EngagementConfiguration : IEntityTypeConfiguration<Engagement>
    {
        public void Configure(EntityTypeBuilder<Engagement> builder)
        {
            builder.HasOne(e => e.Url)
                .WithMany(u => u.Engagements)
                .HasForeignKey(e => e.UrlId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(e => e.CreatedAt)
                .IsRequired();
        }
    }
}
