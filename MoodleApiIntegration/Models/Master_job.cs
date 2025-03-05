using System.ComponentModel.DataAnnotations;

namespace MoodleApiIntegration.Models
{
    public class Master_job
    {
        [Key]
        public int Master_ID { get; set; }
        public DateTime date_run { get; set; }

        public int Current_status { get; set; }
        public string ServerIP { get; set; }=string.Empty;
    
    }
}
