using StudentManagementWebAPI.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Grade
{
    //Validation
    public int GradeId { get; set; }

    [Required]
    [Range(1, 10)]
    public int Level { get; set; }

    [Required]
    [MaxLength(50)]
    public string? ClassName { get; set; }

    public ICollection<Student>? Students { get; set; }
    public ICollection<Course>? Courses { get; set; }
}
