using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementWebAPI.Models;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using StudentManagementWebAPI.Data;

namespace StudentManagementWebAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CoursesController : ControllerBase
    {
        private readonly DataContext _context;

        public CoursesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<Course>>> Get(
           [FromQuery] int pageNumber = 1,
           [FromQuery] int pageSize = 10,
           [FromQuery] string sortBy = "CourseId",
           [FromQuery] string search = "")
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("pageNumber and pageSize must be positive integers.");
            }

            var query = _context.Courses.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.Name != null && c.Name.Contains(search));
            }

            var totalCount = await query.CountAsync();

            var courses = await query.OrderBy(c => EF.Property<object>(c, sortBy))
                                      .Skip((pageNumber - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToListAsync();

            var result = new PagedResult<Course>
            {
                Items = courses,
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = pageNumber,
            };

            var paginationHeader = new
            {
                result.Items,
                result.TotalCount,
                result.PageSize,
                result.CurrentPage,
                result.TotalPages,
                FirstPage = 1,
                LastPage = result.TotalPages,
                NextPage = result.CurrentPage < result.TotalPages ? result.CurrentPage + 1 : (int?)null,
                PrevPage = result.CurrentPage > 1 ? result.CurrentPage - 1 : (int?)null
            };

            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(paginationHeader);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> Get(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return course;
        }

        [HttpPost]
        public async Task<ActionResult<Course>> Post(Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = course.CourseId }, course);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int courseId, Course updatedCourse)
        {
            if (courseId != updatedCourse.CourseId)
            {
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(updatedCourse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Courses.Any(e => e.CourseId == courseId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
            {
                return NotFound();
            }
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
