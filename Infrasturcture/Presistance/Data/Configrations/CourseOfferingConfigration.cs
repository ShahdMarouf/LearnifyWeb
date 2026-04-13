using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Presistance.Data.Configrations
{
    public class CourseOfferingConfiguration
        : IEntityTypeConfiguration<CourseOffering>
    {
   
        public void Configure(EntityTypeBuilder<CourseOffering> builder)
        {
            builder.ToTable("CourseOfferings");

            // Primary Key
            builder.HasKey(co => co.Id);

            // Relation with Course
            builder.HasOne(co => co.Course)
                   .WithMany(c => c.Offerings)
                   .HasForeignKey(co => co.CourseCode)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(co => co.CourseCode)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(co => co.Semester)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(co => co.AcademicYear)
                   .IsRequired()
                   .HasMaxLength(20);

            // منع تكرار نفس المادة في نفس السمستر والسنة
            builder.HasIndex(co => new
            {
                co.CourseCode,
                co.Semester,
                co.AcademicYear
            }).IsUnique();
        }
    }
}