using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementWebAPI.Models;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using StudentManagementWebAPI.Data;


namespace StudentManagementWebAPI.Controllers
{
    //Versioning
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class StudentsController : ControllerBase
    {
        private readonly DataContext _context;  // Giả sử DataContext là lớp DbContext

        public StudentsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet] //lấy danh sách sinh viên

        //Sử dụng tham số pageNumber, pageSize, sortBy, và search để Paging và tìm kiếm
        public async Task<ActionResult<PagedResult<Student>>> Get(
           [FromQuery] int pageNumber = 1,
           [FromQuery] int pageSize = 10,
           [FromQuery] string sortBy = "Id",
           [FromQuery] string search = "")
        {
            //Validation
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("pageNumber and pageSize must be positive integers.");
            }

            var query = _context.Students.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.Name != null && s.Name.Contains(search));
            }

            var totalCount = await query.CountAsync();

            var students = await query.OrderBy(s => EF.Property<object>(s, sortBy))
                                      .Skip((pageNumber - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToListAsync();

            //trả kết quả
            var result = new PagedResult<Student>
            {
                Items = students,
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = pageNumber,
            };

            //Content Negotiation/Header: Dữ liệu trả về được định dạng và trả về thông qua header X-Pagination
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
            // Thông tin Paging được đặt trong header của phản hồi HTTP với tên X-Pagination.
            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(paginationHeader);

            // Phương thức trả về một phản hồi HTTP OK (200) chứa danh sách sinh viên và thông tin Paging
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> Get(int id)
        {
            var student = await _context.Students.FindAsync(id);
            //Exception Handling: Không tìm thấy sinh viên với id tương ứng, phương thức trả về một kết quả NotFound.
            if (student == null)
            {
                return NotFound();
            }
            return student;
        }

        [HttpPost]
        public async Task<ActionResult<Student>> Post(Student student)
        {
            //Validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = student.StudentId }, student);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int studentId, Student updatedStudent)
        {
            if (studentId != updatedStudent.StudentId)
            {
                return BadRequest("ID mismatch");
            }

            //Validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(updatedStudent).State = EntityState.Modified;

            //Exception Handling: Xử lý ngoại lệ DbUpdateConcurrencyException khi lưu thay đổi vào cơ sở dữ liệu
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Students.Any(e => e.StudentId == studentId))
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
        public async Task<IActionResult> Delete(int studentId)
        {
            var student = await _context.Students.FindAsync(studentId);
            //Exception Handling: Không tìm thấy sinh viên với id tương ứng, phương thức trả về một kết quả NotFound.
            if (student == null)
            {
                return NotFound();
            }
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
