namespace SchoolApp.Application.DTOs;

public class CourseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Credits { get; set; }
    public int Capacity { get; set; }
    public bool IsActive { get; set; }
    public int TeacherId { get; set; }
    public string TeacherFullName { get; set; } = string.Empty;
    public int EnrolledCount { get; set; }
}

public class CreateCourseDto
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Credits { get; set; }
    public int Capacity { get; set; }
    public int TeacherId { get; set; }
}

public class UpdateCourseDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Credits { get; set; }
    public int Capacity { get; set; }
    public int TeacherId { get; set; }
    public bool IsActive { get; set; }
}
