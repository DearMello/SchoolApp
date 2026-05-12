using SchoolApp.Domain.Entities;

namespace SchoolApp.Domain.Interfaces;

public interface IStudentRepository : IRepository<Student>
{
    Task<Student?> GetByEmailAsync(string email);
    Task<Student?> GetByStudentNumberAsync(string studentNumber);
    Task<IEnumerable<Student>> GetActiveStudentsAsync();
}

public interface ITeacherRepository : IRepository<Teacher>
{
    Task<Teacher?> GetByEmailAsync(string email);
    Task<IEnumerable<Teacher>> GetByDepartmentAsync(string department);
}

public interface ICourseRepository : IRepository<Course>
{
    Task<Course?> GetByCodeAsync(string code);
    Task<IEnumerable<Course>> GetByTeacherIdAsync(int teacherId);
    Task<Course?> GetWithDetailsAsync(int id);
    Task<IEnumerable<Course>> GetActiveCoursesAsync();
}

public interface IEnrollmentRepository : IRepository<Enrollment>
{
    Task<IEnumerable<Enrollment>> GetByStudentIdAsync(int studentId);
    Task<IEnumerable<Enrollment>> GetByCourseIdAsync(int courseId);
    Task<Enrollment?> GetWithDetailsAsync(int id);
    Task<bool> IsAlreadyEnrolledAsync(int studentId, int courseId);
    Task<int> GetEnrollmentCountByCourseAsync(int courseId);
}

public interface IGradeRepository : IRepository<Grade>
{
    Task<Grade?> GetByEnrollmentIdAsync(int enrollmentId);
    Task<IEnumerable<Grade>> GetByStudentIdAsync(int studentId);
}
