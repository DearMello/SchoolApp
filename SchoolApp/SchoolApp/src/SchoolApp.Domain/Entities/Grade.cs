namespace SchoolApp.Domain.Entities;

public class Grade : BaseEntity
{
    public int EnrollmentId { get; set; }
    public Enrollment Enrollment { get; set; } = null!;

    public decimal Score { get; set; }
    public string LetterGrade { get; set; } = string.Empty;
    public string? Remarks { get; set; }
    public DateTime GradedAt { get; set; } = DateTime.UtcNow;
}
