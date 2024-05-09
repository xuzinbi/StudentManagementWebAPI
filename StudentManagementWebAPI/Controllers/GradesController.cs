using Microsoft.AspNetCore.Mvc;
using StudentManagementWebAPI.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudentManagementWebAPI.Models;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class GradesController : ControllerBase
{
    private readonly IGradeService _gradeService;

    public GradesController(IGradeService gradeService)
    {
        _gradeService = gradeService ?? throw new ArgumentNullException(nameof(gradeService));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Grade>>> Get()
    {
        var grades = await _gradeService.GetAllGrades();
        return Ok(grades);
    }

    [HttpGet("{gradeId}")]
    public async Task<ActionResult<Grade>> Get(int gradeId)
    {
        var grade = await _gradeService.GetGradeById(gradeId);
        if (grade == null)
        {
            return NotFound();
        }

        return Ok(grade);
    }

    [HttpPost]
    public async Task<ActionResult<Grade>> Post(Grade grade)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdGrade = await _gradeService.CreateGrade(grade);
        return CreatedAtAction(nameof(Get), new { gradeId = createdGrade.GradeId }, createdGrade);
    }

    [HttpPut("{gradeId}")]
    public async Task<IActionResult> Put(int gradeId, Grade grade)
    {
        if (gradeId != grade.GradeId)
        {
            return BadRequest("ID mismatch");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updatedGrade = await _gradeService.UpdateGrade(gradeId, grade);
        if (updatedGrade == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{gradeId}")]
    public async Task<IActionResult> Delete(int gradeId)
    {
        var success = await _gradeService.DeleteGrade(gradeId);
        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
}
