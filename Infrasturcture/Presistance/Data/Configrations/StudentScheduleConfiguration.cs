using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Presistance.Data.Configrations
{
    public class StudentScheduleConfiguration : IEntityTypeConfiguration<StudentSchedule>
    {
        public void Configure(EntityTypeBuilder<StudentSchedule> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.StudentCourse)
                   .WithMany(sc => sc.StudentSchedules)
                   .HasForeignKey(x => x.StudentCourseId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Day).IsRequired().HasMaxLength(20);
            builder.Property(x => x.Type).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Hall).IsRequired().HasMaxLength(100);
        }
    }
}
