using SchoolApp.Application.DTOs;

namespace SchoolApp.Application.Interfaces;

public interface IStudentService
{
    Task<IEnumerable<StudentDto>> GetAllAsync();
    Task<StudentDto?> GetByIdAsync(int id);
    Task<StudentDto?> GetByStudentNumberAsync(string studentNumber);
    Task<IEnumerable<StudentDto>> GetActiveStudentsAsync();
    Task<StudentDto> CreateAsync(CreateStudentDto dto);
    Task<StudentDto?> UpdateAsync(int id, UpdateStudentDto dto);
    Task<bool> DeleteAsync(int id);
}

public interface ITeacherService
{
    Task<IEnumerable<TeacherDto>> GetAllAsync();
    Task<TeacherDto?> GetByIdAsync(int id);
    Task<IEnumerable<TeacherDto>> GetByDepartmentAsync(string department);
    Task<TeacherDto> CreateAsync(CreateTeacherDto dto);
    Task<TeacherDto?> UpdateAsync(int id, UpdateTeacherDto dto);
    Task<bool> DeleteAsync(int id);
}

public interface ICourseService
{
    Task<IEnumerable<CourseDto>> GetAllAsync();
    Task<CourseDto?> GetByIdAsync(int id);
    Task<IEnumerable<CourseDto>> GetByTeacherIdAsync(int teacherId);
    Task<IEnumerable<CourseDto>> GetActiveCoursesAsync();
    Task<CourseDto> CreateAsync(CreateCourseDto dto);
    Task<CourseDto?> UpdateAsync(int id, UpdateCourseDto dto);
    Task<bool> DeleteAsync(int id);
}

public interface IEnrollmentService
{
    Task<IEnumerable<EnrollmentDto>> GetAllAsync();
    Task<EnrollmentDto?> GetByIdAsync(int id);
    Task<IEnumerable<EnrollmentDto>> GetByStudentIdAsync(int studentId);
    Task<IEnumerable<EnrollmentDto>> GetByCourseIdAsync(int courseId);
    Task<EnrollmentDto> EnrollAsync(CreateEnrollmentDto dto);
    Task<bool> DropAsync(int id);
    Task<GradeDto> AssignGradeAsync(AssignGradeDto dto);
}
