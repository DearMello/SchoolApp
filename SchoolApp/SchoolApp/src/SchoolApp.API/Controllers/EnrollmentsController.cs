using Microsoft.AspNetCore.Mvc;
using SchoolApp.Application.DTOs;
using SchoolApp.Application.Interfaces;

namespace SchoolApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _service;

    public EnrollmentsController(IEnrollmentService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("student/{studentId:int}")]
    public async Task<IActionResult> GetByStudent(int studentId)
        => Ok(await _service.GetByStudentIdAsync(studentId));

    [HttpGet("course/{courseId:int}")]
    public async Task<IActionResult> GetByCourse(int courseId)
        => Ok(await _service.GetByCourseIdAsync(courseId));

    [HttpPost]
    public async Task<IActionResult> Enroll([FromBody] CreateEnrollmentDto dto)
    {
        try
        {
            var result = await _service.EnrollAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("{id:int}/drop")]
    public async Task<IActionResult> Drop(int id)
    {
        var success = await _service.DropAsync(id);
        return success ? NoContent() : NotFound();
    }

    [HttpPost("grades")]
    public async Task<IActionResult> AssignGrade([FromBody] AssignGradeDto dto)
    {
        var result = await _service.AssignGradeAsync(dto);
        return Ok(result);
    }
}
