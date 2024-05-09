using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using StudentManagementWebAPI.Models;

public class Course
{
    //Validation
    [Required]
    public int CourseId { get; set; }

    [Required]
    [MaxLength(50)]
    public string? Name { get; set; }

    [Required]
    public int GradeId { get; set; }
    public Grade? Grade { get; set; }

    public ICollection<StudentCourse>? StudentCourses { get; set; }
}
