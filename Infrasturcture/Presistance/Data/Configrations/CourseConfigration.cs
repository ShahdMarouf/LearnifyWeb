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
    public class CourseConfigration:IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses");

            // Primary Key
            builder.HasKey(c => c.CourseCode);

            builder.Property(c => c.CourseCode)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(c => c.FullName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(c => c.Credits)
                   .IsRequired();

            // Relationship handled in StudentCourseConfig
        }
    }
}
