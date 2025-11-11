using Linksy.Domain.Entities.Tracking;
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
            builder.UseTpcMappingStrategy();
            builder.Property(e => e.IpAddress)
                .HasMaxLength(64);
            builder.Property(e => e.EngagedAt)
                .IsRequired();
        }
    }
}
