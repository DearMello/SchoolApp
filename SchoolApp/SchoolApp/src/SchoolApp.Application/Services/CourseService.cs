using SchoolApp.Application.DTOs;
using SchoolApp.Application.Interfaces;
using SchoolApp.Domain.Entities;
using SchoolApp.Domain.Interfaces;

namespace SchoolApp.Application.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _repo;
    private readonly IEnrollmentRepository _enrollmentRepo;

    public CourseService(ICourseRepository repo, IEnrollmentRepository enrollmentRepo)
    {
        _repo = repo;
        _enrollmentRepo = enrollmentRepo;
    }

    public async Task<IEnumerable<CourseDto>> GetAllAsync()
    {
        var courses = await _repo.GetAllAsync();
        var result = new List<CourseDto>();
        foreach (var c in courses)
        {
            var count = await _enrollmentRepo.GetEnrollmentCountByCourseAsync(c.Id);
            result.Add(MapToDto(c, count));
        }
        return result;
    }

    public async Task<CourseDto?> GetByIdAsync(int id)
    {
        var c = await _repo.GetWithDetailsAsync(id);
        if (c is null) return null;
        var count = await _enrollmentRepo.GetEnrollmentCountByCourseAsync(id);
        return MapToDto(c, count);
    }

    public async Task<IEnumerable<CourseDto>> GetByTeacherIdAsync(int teacherId)
    {
        var courses = await _repo.GetByTeacherIdAsync(teacherId);
        var result = new List<CourseDto>();
        foreach (var c in courses)
        {
            var count = await _enrollmentRepo.GetEnrollmentCountByCourseAsync(c.Id);
            result.Add(MapToDto(c, count));
        }
        return result;
    }

    public async Task<IEnumerable<CourseDto>> GetActiveCoursesAsync()
    {
        var courses = await _repo.GetActiveCoursesAsync();
        var result = new List<CourseDto>();
        foreach (var c in courses)
        {
            var count = await _enrollmentRepo.GetEnrollmentCountByCourseAsync(c.Id);
            result.Add(MapToDto(c, count));
        }
        return result;
    }

    public async Task<CourseDto> CreateAsync(CreateCourseDto dto)
    {
        var course = new Course
        {
            Name = dto.Name,
            Code = dto.Code,
            Description = dto.Description,
            Credits = dto.Credits,
            Capacity = dto.Capacity,
            TeacherId = dto.TeacherId
        };
        await _repo.AddAsync(course);
        await _repo.SaveChangesAsync();
        return MapToDto(course, 0);
    }

    public async Task<CourseDto?> UpdateAsync(int id, UpdateCourseDto dto)
    {
        var course = await _repo.GetByIdAsync(id);
        if (course is null) return null;

        course.Name = dto.Name;
        course.Description = dto.Description;
        course.Credits = dto.Credits;
        course.Capacity = dto.Capacity;
        course.TeacherId = dto.TeacherId;
        course.IsActive = dto.IsActive;
        course.UpdatedAt = DateTime.UtcNow;

        _repo.Update(course);
        await _repo.SaveChangesAsync();
        var count = await _enrollmentRepo.GetEnrollmentCountByCourseAsync(id);
        return MapToDto(course, count);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var course = await _repo.GetByIdAsync(id);
        if (course is null) return false;
        _repo.Delete(course);
        await _repo.SaveChangesAsync();
        return true;
    }

    private static CourseDto MapToDto(Course c, int enrolledCount) => new()
    {
        Id = c.Id,
        Name = c.Name,
        Code = c.Code,
        Description = c.Description,
        Credits = c.Credits,
        Capacity = c.Capacity,
        IsActive = c.IsActive,
        TeacherId = c.TeacherId,
        TeacherFullName = c.Teacher is not null ? $"{c.Teacher.FirstName} {c.Teacher.LastName}" : string.Empty,
        EnrolledCount = enrolledCount
    };
}
