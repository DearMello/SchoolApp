namespace SchoolApp.Domain.Entities;

public class Teacher : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
