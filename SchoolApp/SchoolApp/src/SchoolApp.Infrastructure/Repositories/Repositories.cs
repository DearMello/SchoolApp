using Microsoft.EntityFrameworkCore;
using SchoolApp.Domain.Entities;
using SchoolApp.Domain.Interfaces;
using SchoolApp.Infrastructure.Data;

namespace SchoolApp.Infrastructure.Repositories;

public class StudentRepository : GenericRepository<Student>, IStudentRepository
{
    public StudentRepository(SchoolDbContext context) : base(context) { }

    public async Task<Student?> GetByEmailAsync(string email)
        => await _context.Students.FirstOrDefaultAsync(s => s.Email.ToLower() == email.ToLower());

    public async Task<Student?> GetByStudentNumberAsync(string studentNumber)
        => await _context.Students.FirstOrDefaultAsync(s => s.StudentNumber == studentNumber);

    public async Task<IEnumerable<Student>> GetActiveStudentsAsync()
        => await _context.Students.Where(s => s.IsActive).ToListAsync();
}

public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
{
    public TeacherRepository(SchoolDbContext context) : base(context) { }

    public async Task<Teacher?> GetByEmailAsync(string email)
        => await _context.Teachers.FirstOrDefaultAsync(t => t.Email.ToLower() == email.ToLower());

    public async Task<IEnumerable<Teacher>> GetByDepartmentAsync(string department)
        => await _context.Teachers
            .Where(t => t.Department.ToLower().Contains(department.ToLower()))
            .ToListAsync();
}

public class CourseRepository : GenericRepository<Course>, ICourseRepository
{
    public CourseRepository(SchoolDbContext context) : base(context) { }

    public async Task<Course?> GetByCodeAsync(string code)
        => await _context.Courses.FirstOrDefaultAsync(c => c.Code == code);

    public async Task<IEnumerable<Course>> GetByTeacherIdAsync(int teacherId)
        => await _context.Courses
            .Include(c => c.Teacher)
            .Where(c => c.TeacherId == teacherId)
            .ToListAsync();

    public async Task<Course?> GetWithDetailsAsync(int id)
        => await _context.Courses
            .Include(c => c.Teacher)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<IEnumerable<Course>> GetActiveCoursesAsync()
        => await _context.Courses
            .Include(c => c.Teacher)
            .Where(c => c.IsActive)
            .ToListAsync();
}

public class EnrollmentRepository : GenericRepository<Enrollment>, IEnrollmentRepository
{
    public EnrollmentRepository(SchoolDbContext context) : base(context) { }

    public async Task<IEnumerable<Enrollment>> GetByStudentIdAsync(int studentId)
        => await _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.Course)
            .Include(e => e.Grade)
            .Where(e => e.StudentId == studentId)
            .ToListAsync();

    public async Task<IEnumerable<Enrollment>> GetByCourseIdAsync(int courseId)
        => await _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.Course)
            .Include(e => e.Grade)
            .Where(e => e.CourseId == courseId)
            .ToListAsync();

    public async Task<Enrollment?> GetWithDetailsAsync(int id)
        => await _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.Course)
            .Include(e => e.Grade)
            .FirstOrDefaultAsync(e => e.Id == id);

    public async Task<bool> IsAlreadyEnrolledAsync(int studentId, int courseId)
        => await _context.Enrollments
            .AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);

    public async Task<int> GetEnrollmentCountByCourseAsync(int courseId)
        => await _context.Enrollments
            .CountAsync(e => e.CourseId == courseId);
}

public class GradeRepository : GenericRepository<Grade>, IGradeRepository
{
    public GradeRepository(SchoolDbContext context) : base(context) { }

    public async Task<Grade?> GetByEnrollmentIdAsync(int enrollmentId)
        => await _context.Grades.FirstOrDefaultAsync(g => g.EnrollmentId == enrollmentId);

    public async Task<IEnumerable<Grade>> GetByStudentIdAsync(int studentId)
        => await _context.Grades
            .Include(g => g.Enrollment)
            .Where(g => g.Enrollment.StudentId == studentId)
            .ToListAsync();
}
