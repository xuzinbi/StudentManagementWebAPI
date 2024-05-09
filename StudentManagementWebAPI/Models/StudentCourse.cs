using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StudentManagementWebAPI.Models;


//Bảng trung gian
public class StudentCourse
{
    [Key, Column(Order = 0)]
    public int StudentId { get; set; }
    [ForeignKey("StudentId")]
    public Student? Student { get; set; }

    [Key, Column(Order = 1)]
    public int CourseId { get; set; }
    [ForeignKey("CourseId")]
    public Course? Course { get; set; }
}
