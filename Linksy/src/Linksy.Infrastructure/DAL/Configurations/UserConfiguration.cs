using Linksy.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.DAL.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.UserName)
                .HasMaxLength(24)
                .IsRequired();
            builder.Property(u => u.FirstName)
                .HasMaxLength(32)
                .IsRequired();
            builder.Property(u => u.LastName)
                .HasMaxLength(32)
                .IsRequired();
            builder.Property(u => u.PasswordHash)
                .IsRequired();
            builder.Property(u => u.CreatedAt)
                .IsRequired();
            builder.Property(u => u.Gender)
                .HasConversion<string>()
                .IsRequired();
        }
    }
}
