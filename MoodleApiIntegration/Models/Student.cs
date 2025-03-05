using System.ComponentModel.DataAnnotations;

namespace MoodleApiIntegration.Models
{
    public class Student
    {
        [Key]
        public int StudentID { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
