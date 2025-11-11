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
    internal class UrlEngagementConfiguration : IEntityTypeConfiguration<UrlEngagement>
    {
        public void Configure(EntityTypeBuilder<UrlEngagement> builder)
        {
            builder.ToTable("UrlEngagements");
            builder.HasOne(u => u.Url)
                .WithMany(u => u.Engagements)
                .HasForeignKey(u => u.UrlId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
