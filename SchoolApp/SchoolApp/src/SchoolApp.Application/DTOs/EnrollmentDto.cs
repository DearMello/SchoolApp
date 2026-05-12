using SchoolApp.Domain.Enums;

namespace SchoolApp.Application.DTOs;

public class EnrollmentDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string StudentFullName { get; set; } = string.Empty;
    public string StudentNumber { get; set; } = string.Empty;
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public string CourseCode { get; set; } = string.Empty;
    public DateTime EnrolledAt { get; set; }
    public EnrollmentStatus Status { get; set; }
    public string StatusName => Status.ToString();
    public GradeDto? Grade { get; set; }
}

public class CreateEnrollmentDto
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
}

public class GradeDto
{
    public int Id { get; set; }
    public int EnrollmentId { get; set; }
    public decimal Score { get; set; }
    public string LetterGrade { get; set; } = string.Empty;
    public string? Remarks { get; set; }
    public DateTime GradedAt { get; set; }
}

public class AssignGradeDto
{
    public int EnrollmentId { get; set; }
    public decimal Score { get; set; }
    public string? Remarks { get; set; }
}
