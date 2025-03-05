using System.ComponentModel.DataAnnotations;

namespace MoodleApiIntegration.Models
{
    public class StudentStopList
    {
        [Key]
        public int StopListID { get; set; }
        public int StudentID { get; set; }
        public int ReasonID { get; set; }
      
        public DateTime DateAdded { get; set; }
        public DateTime? ClearedDate { get; set; }
        public string AddedBy { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
    }
}
