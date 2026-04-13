using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Presistance.Data.Contexts
{
    public class storeDbContext : DbContext
    {
        public storeDbContext(DbContextOptions<storeDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<CourseMaterial> CourseMaterials { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<CourseOffering> CourseOffering { get; set; }
        public DbSet<StudentSchedule> StudentSchedules { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
