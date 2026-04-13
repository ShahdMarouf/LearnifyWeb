using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Presistance.Data.Configrations
{
    public class CourseMaterialConfiguration : IEntityTypeConfiguration<CourseMaterial>
    {
        public void Configure(EntityTypeBuilder<CourseMaterial> builder)
        {
            builder.ToTable("CourseMaterials");

            builder.HasKey(cm => cm.Id);

            builder.Property(cm => cm.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(cm => cm.Type)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(cm => cm.FilePath)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(cm => cm.WeekNumber)
                   .IsRequired();

            builder.Property(cm => cm.CourseCode)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(cm => cm.InstructorId)
                   .IsRequired();
        }
    }
}
