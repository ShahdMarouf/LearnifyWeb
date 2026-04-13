using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistance.Data.Configrations
{
    public class UserConfigration : IEntityTypeConfiguration<User>
    {
      

        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder.HasKey(u => u.Id);

            builder.Property(u => u.FullName)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.HasIndex(u => u.Email)
                   .IsUnique();

            builder.HasDiscriminator<string>("UserType")
                   .HasValue<Student>("Student")
                   .HasValue<Instructor>("Instructor");
        }
    }

    
}
