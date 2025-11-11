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
    internal class UmtParameterEngagementConfiguration : IEntityTypeConfiguration<UmtParameterEngagement>
    {
        public void Configure(EntityTypeBuilder<UmtParameterEngagement> builder)
        {
            builder.ToTable("UmtParameterEngagements"); 
            builder.HasOne(u => u.UmtParameter)
                .WithMany(u => u.Engagements)
                .HasForeignKey(u => u.UmtParameterId);
            builder.Property(u => u.EngagedAt)
                .IsRequired();
        }
    }
}
