using SchoolApp.Domain.Enums;

namespace SchoolApp.Domain.Entities;

public class Enrollment : BaseEntity
{
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;

    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;

    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;

    public Grade? Grade { get; set; }
}
