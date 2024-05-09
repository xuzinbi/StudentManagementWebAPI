using StudentManagementWebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagementWebAPI.Services
{
    public interface IGradeService
    {
        Task<Grade> GetGradeById(int gradeId);
        Task<IEnumerable<Grade>> GetAllGrades();
        Task<Grade> CreateGrade(Grade grade);
        Task<Grade?> UpdateGrade(int gradeId, Grade updatedGrade);
        Task<bool> DeleteGrade(int gradeId);
    }
}
