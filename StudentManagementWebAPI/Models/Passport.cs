using System.ComponentModel.DataAnnotations;

namespace StudentManagementWebAPI.Models
{
    public class Passport
    {
        [Key]
        public int PassportId { get; set; }

        [Required]
        public string? PassportNumber { get; set; }

        [Required]
        public string? ExpiryDate { get; set; }

        // Mối quan hệ ngược lại với Student
        public Student? Student { get; set; }
    }
}
