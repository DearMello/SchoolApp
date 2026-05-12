using SchoolApp.Application.DTOs;
using SchoolApp.Application.Interfaces;
using SchoolApp.Domain.Entities;
using SchoolApp.Domain.Interfaces;

namespace SchoolApp.Application.Services;

public class TeacherService : ITeacherService
{
    private readonly ITeacherRepository _repo;

    public TeacherService(ITeacherRepository repo) => _repo = repo;

    public async Task<IEnumerable<TeacherDto>> GetAllAsync()
        => (await _repo.GetAllAsync()).Select(MapToDto);

    public async Task<TeacherDto?> GetByIdAsync(int id)
    {
        var t = await _repo.GetByIdAsync(id);
        return t is null ? null : MapToDto(t);
    }

    public async Task<IEnumerable<TeacherDto>> GetByDepartmentAsync(string department)
        => (await _repo.GetByDepartmentAsync(department)).Select(MapToDto);

    public async Task<TeacherDto> CreateAsync(CreateTeacherDto dto)
    {
        var teacher = new Teacher
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Phone = dto.Phone,
            Department = dto.Department
        };
        await _repo.AddAsync(teacher);
        await _repo.SaveChangesAsync();
        return MapToDto(teacher);
    }

    public async Task<TeacherDto?> UpdateAsync(int id, UpdateTeacherDto dto)
    {
        var teacher = await _repo.GetByIdAsync(id);
        if (teacher is null) return null;

        teacher.FirstName = dto.FirstName;
        teacher.LastName = dto.LastName;
        teacher.Email = dto.Email;
        teacher.Phone = dto.Phone;
        teacher.Department = dto.Department;
        teacher.IsActive = dto.IsActive;
        teacher.UpdatedAt = DateTime.UtcNow;

        _repo.Update(teacher);
        await _repo.SaveChangesAsync();
        return MapToDto(teacher);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var teacher = await _repo.GetByIdAsync(id);
        if (teacher is null) return false;
        _repo.Delete(teacher);
        await _repo.SaveChangesAsync();
        return true;
    }

    private static TeacherDto MapToDto(Teacher t) => new()
    {
        Id = t.Id,
        FirstName = t.FirstName,
        LastName = t.LastName,
        Email = t.Email,
        Phone = t.Phone,
        Department = t.Department,
        IsActive = t.IsActive
    };
}
