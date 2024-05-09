using Microsoft.EntityFrameworkCore;
using StudentManagementWebAPI.Data;

namespace StudentManagementWebAPI.Services
{
    public class CourseService : ICourseService
    {
        private readonly DataContext _context;

        public CourseService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetCourses()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<Course> GetCourseById(int courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
            {
                // Xử lý khi không tìm thấy khóa học
                throw new ArgumentException("Course not found", nameof(courseId));
            }

            return course;
        }

        public async Task<IEnumerable<Course>> GetAllCourses()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<Course> CreateCourse(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<Course> UpdateCourse(int courseId, Course updatedCourse)
        {
            var existingCourse = await _context.Courses.FindAsync(courseId);
            if (existingCourse == null)
            {
                // Xử lý khi không tìm thấy khóa học
                throw new ArgumentException("Course not found", nameof(courseId));
            }

            existingCourse.Name = updatedCourse.Name;
            existingCourse.GradeId = updatedCourse.GradeId;

            await _context.SaveChangesAsync();
            return existingCourse;
        }


        public async Task<bool> DeleteCourse(int courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
            {
                return false;
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
