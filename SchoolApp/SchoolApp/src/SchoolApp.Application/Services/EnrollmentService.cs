using SchoolApp.Application.DTOs;
using SchoolApp.Application.Interfaces;
using SchoolApp.Domain.Entities;
using SchoolApp.Domain.Enums;
using SchoolApp.Domain.Interfaces;

namespace SchoolApp.Application.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _repo;
    private readonly IGradeRepository _gradeRepo;
    private readonly ICourseRepository _courseRepo;

    public EnrollmentService(IEnrollmentRepository repo, IGradeRepository gradeRepo, ICourseRepository courseRepo)
    {
        _repo = repo;
        _gradeRepo = gradeRepo;
        _courseRepo = courseRepo;
    }

    public async Task<IEnumerable<EnrollmentDto>> GetAllAsync()
        => (await _repo.GetAllAsync()).Select(MapToDto);

    public async Task<EnrollmentDto?> GetByIdAsync(int id)
    {
        var e = await _repo.GetWithDetailsAsync(id);
        return e is null ? null : MapToDto(e);
    }

    public async Task<IEnumerable<EnrollmentDto>> GetByStudentIdAsync(int studentId)
        => (await _repo.GetByStudentIdAsync(studentId)).Select(MapToDto);

    public async Task<IEnumerable<EnrollmentDto>> GetByCourseIdAsync(int courseId)
        => (await _repo.GetByCourseIdAsync(courseId)).Select(MapToDto);

    public async Task<EnrollmentDto> EnrollAsync(CreateEnrollmentDto dto)
    {
        var alreadyEnrolled = await _repo.IsAlreadyEnrolledAsync(dto.StudentId, dto.CourseId);
        if (alreadyEnrolled)
            throw new InvalidOperationException("Student is already enrolled in this course.");

        var course = await _courseRepo.GetByIdAsync(dto.CourseId)
            ?? throw new InvalidOperationException("Course not found.");

        var currentCount = await _repo.GetEnrollmentCountByCourseAsync(dto.CourseId);
        if (currentCount >= course.Capacity)
            throw new InvalidOperationException("Course has reached maximum capacity.");

        var enrollment = new Enrollment
        {
            StudentId = dto.StudentId,
            CourseId = dto.CourseId,
            EnrolledAt = DateTime.UtcNow,
            Status = EnrollmentStatus.Active
        };

        await _repo.AddAsync(enrollment);
        await _repo.SaveChangesAsync();
        return MapToDto(enrollment);
    }

    public async Task<bool> DropAsync(int id)
    {
        var enrollment = await _repo.GetByIdAsync(id);
        if (enrollment is null) return false;

        enrollment.Status = EnrollmentStatus.Dropped;
        enrollment.UpdatedAt = DateTime.UtcNow;
        _repo.Update(enrollment);
        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task<GradeDto> AssignGradeAsync(AssignGradeDto dto)
    {
        var existing = await _gradeRepo.GetByEnrollmentIdAsync(dto.EnrollmentId);
        if (existing is not null)
        {
            existing.Score = dto.Score;
            existing.LetterGrade = CalculateLetterGrade(dto.Score);
            existing.Remarks = dto.Remarks;
            existing.GradedAt = DateTime.UtcNow;
            existing.UpdatedAt = DateTime.UtcNow;
            _gradeRepo.Update(existing);
            await _gradeRepo.SaveChangesAsync();
            return MapGradeToDto(existing);
        }

        var grade = new Grade
        {
            EnrollmentId = dto.EnrollmentId,
            Score = dto.Score,
            LetterGrade = CalculateLetterGrade(dto.Score),
            Remarks = dto.Remarks,
            GradedAt = DateTime.UtcNow
        };

        await _gradeRepo.AddAsync(grade);
        await _gradeRepo.SaveChangesAsync();
        return MapGradeToDto(grade);
    }

    private static string CalculateLetterGrade(decimal score) => score switch
    {
        >= 90 => "A",
        >= 80 => "B",
        >= 70 => "C",
        >= 60 => "D",
        _ => "F"
    };

    private static EnrollmentDto MapToDto(Enrollment e) => new()
    {
        Id = e.Id,
        StudentId = e.StudentId,
        StudentFullName = e.Student is not null ? $"{e.Student.FirstName} {e.Student.LastName}" : string.Empty,
        StudentNumber = e.Student?.StudentNumber ?? string.Empty,
        CourseId = e.CourseId,
        CourseName = e.Course?.Name ?? string.Empty,
        CourseCode = e.Course?.Code ?? string.Empty,
        EnrolledAt = e.EnrolledAt,
        Status = e.Status,
        Grade = e.Grade is not null ? MapGradeToDto(e.Grade) : null
    };

    private static GradeDto MapGradeToDto(Grade g) => new()
    {
        Id = g.Id,
        EnrollmentId = g.EnrollmentId,
        Score = g.Score,
        LetterGrade = g.LetterGrade,
        Remarks = g.Remarks,
        GradedAt = g.GradedAt
    };
}
