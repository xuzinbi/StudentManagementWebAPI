using Microsoft.EntityFrameworkCore;
using StudentManagementWebAPI.Data;
using StudentManagementWebAPI.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementWebAPI.Services
{
    public class GradeService : IGradeService
    {
        private readonly DataContext _context;

        public GradeService(DataContext context)
        {
            _context = context;
        }

        public async Task<Grade> GetGradeById(int gradeId)
        {
            var grade = await _context.Grades.FindAsync(gradeId);
            return grade ?? new Grade();
        }

        public async Task<IEnumerable<Grade>> GetAllGrades()
        {
            return await _context.Grades.ToListAsync();
        }

        public async Task<Grade> CreateGrade(Grade grade)
        {
            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();
            return grade;
        }

        public async Task<Grade?> UpdateGrade(int gradeId, Grade updatedGrade)
        {
            var existingGrade = await _context.Grades.FindAsync(gradeId);
            if (existingGrade == null)
            {
                return null;
            }

            existingGrade.Level = updatedGrade.Level;
            existingGrade.ClassName = updatedGrade.ClassName;

            await _context.SaveChangesAsync();
            return existingGrade;
        }

        public async Task<bool> DeleteGrade(int gradeId)
        {
            var grade = await _context.Grades.FindAsync(gradeId);
            if (grade == null)
            {
                return false;
            }

            _context.Grades.Remove(grade);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
