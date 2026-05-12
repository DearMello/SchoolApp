using SchoolApp.Application.DTOs;
using SchoolApp.Application.Interfaces;
using SchoolApp.Domain.Entities;
using SchoolApp.Domain.Interfaces;

namespace SchoolApp.Application.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _repo;

    public StudentService(IStudentRepository repo) => _repo = repo;

    public async Task<IEnumerable<StudentDto>> GetAllAsync()
        => (await _repo.GetAllAsync()).Select(MapToDto);

    public async Task<StudentDto?> GetByIdAsync(int id)
    {
        var s = await _repo.GetByIdAsync(id);
        return s is null ? null : MapToDto(s);
    }

    public async Task<StudentDto?> GetByStudentNumberAsync(string studentNumber)
    {
        var s = await _repo.GetByStudentNumberAsync(studentNumber);
        return s is null ? null : MapToDto(s);
    }

    public async Task<IEnumerable<StudentDto>> GetActiveStudentsAsync()
        => (await _repo.GetActiveStudentsAsync()).Select(MapToDto);

    public async Task<StudentDto> CreateAsync(CreateStudentDto dto)
    {
        var student = new Student
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Phone = dto.Phone,
            DateOfBirth = dto.DateOfBirth,
            StudentNumber = dto.StudentNumber
        };
        await _repo.AddAsync(student);
        await _repo.SaveChangesAsync();
        return MapToDto(student);
    }

    public async Task<StudentDto?> UpdateAsync(int id, UpdateStudentDto dto)
    {
        var student = await _repo.GetByIdAsync(id);
        if (student is null) return null;

        student.FirstName = dto.FirstName;
        student.LastName = dto.LastName;
        student.Email = dto.Email;
        student.Phone = dto.Phone;
        student.IsActive = dto.IsActive;
        student.UpdatedAt = DateTime.UtcNow;

        _repo.Update(student);
        await _repo.SaveChangesAsync();
        return MapToDto(student);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var student = await _repo.GetByIdAsync(id);
        if (student is null) return false;
        _repo.Delete(student);
        await _repo.SaveChangesAsync();
        return true;
    }

    private static StudentDto MapToDto(Student s) => new()
    {
        Id = s.Id,
        FirstName = s.FirstName,
        LastName = s.LastName,
        Email = s.Email,
        Phone = s.Phone,
        DateOfBirth = s.DateOfBirth,
        StudentNumber = s.StudentNumber,
        IsActive = s.IsActive
    };
}
