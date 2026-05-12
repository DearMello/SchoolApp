namespace SchoolApp.Domain.Entities;

public class Course : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Credits { get; set; }
    public int Capacity { get; set; }
    public bool IsActive { get; set; } = true;

    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; } = null!;

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
