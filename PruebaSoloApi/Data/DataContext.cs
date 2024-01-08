using Microsoft.EntityFrameworkCore;
using PruebaSoloApi.Entities;

namespace PruebaSoloApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Teacher)
                .WithOne(t => t.Course)
                .HasForeignKey<Teacher>(t => t.CourseId);

            modelBuilder.Entity<Student>()
                .HasMany(e => e.Courses)
                .WithMany(e => e.Students);
        }
    }
}
