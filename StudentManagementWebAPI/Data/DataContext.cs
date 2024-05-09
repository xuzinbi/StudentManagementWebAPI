using Microsoft.EntityFrameworkCore;
using StudentManagementWebAPI.Models;

namespace StudentManagementWebAPI.Data
{
    //Entity Framework Core
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<StudentCourse> StudentCourses { get; set; } = null!;
        public DbSet<Grade> Grades { get; set; } = null!;
        public DbSet<Passport> Passports { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Passport>()
                .HasOne(p => p.Student)
                .WithOne(s => s.Passport)
                .HasForeignKey<Student>(s => s.PassportId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Passport>()
                .Property(p => p.PassportNumber)
                .IsRequired();

            modelBuilder.Entity<Passport>()
                .Property(p => p.ExpiryDate)
                .IsRequired();

        }
    }
}

