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
    internal class LandingPageConfiguration : IEntityTypeConfiguration<LandingPage>
    {
        public void Configure(EntityTypeBuilder<LandingPage> builder)
        {
            builder.HasOne(l => l.Url)
                .WithOne(u => u.LandingPage)
                .HasForeignKey<LandingPage>(l => l.UrlId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(l => l.Title)
                .HasMaxLength(128)
                .IsRequired();
            builder.Property(l => l.Description)
                .HasMaxLength(512);
            builder.Property(l => l.BackgroundColor)
                .HasMaxLength(64);
        }
    }
}
