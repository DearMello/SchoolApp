using Microsoft.EntityFrameworkCore;
using SchoolApp.Domain.Entities;

namespace SchoolApp.Infrastructure.Data;

public class SchoolDbContext : DbContext
{
    public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options) { }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<Grade> Grades => Set<Grade>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Student>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            e.Property(x => x.LastName).IsRequired().HasMaxLength(100);
            e.Property(x => x.Email).HasMaxLength(200);
            e.Property(x => x.StudentNumber).IsRequired().HasMaxLength(20);
            e.HasIndex(x => x.StudentNumber).IsUnique();
            e.HasIndex(x => x.Email).IsUnique();
        });

        modelBuilder.Entity<Teacher>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            e.Property(x => x.LastName).IsRequired().HasMaxLength(100);
            e.Property(x => x.Email).HasMaxLength(200);
            e.Property(x => x.Department).HasMaxLength(150);
            e.HasIndex(x => x.Email).IsUnique();
        });

        modelBuilder.Entity<Course>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(200);
            e.Property(x => x.Code).IsRequired().HasMaxLength(20);
            e.Property(x => x.Description).HasMaxLength(1000);
            e.HasIndex(x => x.Code).IsUnique();
            e.HasOne(x => x.Teacher)
                .WithMany(x => x.Courses)
                .HasForeignKey(x => x.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Enrollment>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.Student)
                .WithMany(x => x.Enrollments)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Course)
                .WithMany(x => x.Enrollments)
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasIndex(x => new { x.StudentId, x.CourseId }).IsUnique();
        });

        modelBuilder.Entity<Grade>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Score).HasColumnType("decimal(5,2)");
            e.Property(x => x.LetterGrade).HasMaxLength(2);
            e.Property(x => x.Remarks).HasMaxLength(500);
            e.HasOne(x => x.Enrollment)
                .WithOne(x => x.Grade)
                .HasForeignKey<Grade>(x => x.EnrollmentId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
