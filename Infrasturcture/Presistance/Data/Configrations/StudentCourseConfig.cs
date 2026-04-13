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
    public class StudentCourseConfig : IEntityTypeConfiguration<StudentCourse>
    {
        public void Configure(EntityTypeBuilder<StudentCourse> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Student)
                   .WithMany(s => s.StudentCourses)
                   .HasForeignKey(x => x.StudentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.CourseCode)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(x => x.Semester)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(x => x.AcademicYear)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(x => x.Points)
                   .HasPrecision(3, 2);

            builder.HasIndex(x => new
            {
                x.StudentId,
                x.CourseCode,
                x.Semester,
                x.AcademicYear
            }).IsUnique();
        }
    }
}
