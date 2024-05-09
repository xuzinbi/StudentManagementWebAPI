using StudentManagementWebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagementWebAPI.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetCourses();
        Task<Course> GetCourseById(int courseId);
        Task<IEnumerable<Course>> GetAllCourses();
        Task<Course> CreateCourse(Course course);
        Task<Course> UpdateCourse(int courseId, Course updatedCourse);
        Task<bool> DeleteCourse(int courseId);
    }
}
