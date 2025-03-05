using System.ComponentModel.DataAnnotations;

namespace MoodleApiIntegration.Models
{
    public class Job_Log
    {
        [Key]
        public int LogID { get; set; }
        public int Master_ID { get; set; }
        public int student_id { get; set; }
        public string exception_message { get; set; }=string.Empty;
        public bool IS_SUCCESS { get; set; }
    
    }
}
