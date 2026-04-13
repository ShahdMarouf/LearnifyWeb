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
    public class StudentConfigration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.Property(s => s.StudentAcademicId)
             .IsRequired();

            builder.HasIndex(s => s.StudentAcademicId)
                   .IsUnique();

            builder.Property(s => s.GPA)
                   .HasColumnType("decimal(3,2)");

            builder.Property(s => s.Percentage)
                   .HasColumnType("decimal(5,2)");

            builder.HasOne(s => s.Instructor)
                   .WithMany(i => i.Students)
                   .HasForeignKey(s => s.InstructorId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    

     }
}
