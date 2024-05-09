using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace StudentManagementWebAPI.Models;
public class Student
{
    public Student() { } // Constructor mặc định

    public Student(int studentId, string name, int gradeId)
    {
        StudentId = studentId;
        Name = name;
        GradeId = gradeId;
    }

    [Required]
    public int StudentId { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [MaxLength(50, ErrorMessage = "Name must be at most 50 characters long")]
    public string? Name { get; set; }

    [Required]
    public int GradeId { get; set; }
    public Grade? Grade { get; set; }
    public int PassportId { get; set; }
    public Passport? Passport { get; set; }

    public ICollection<StudentCourse>? StudentCourses { get; set; }

    
}
