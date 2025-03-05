using System.ComponentModel.DataAnnotations;

namespace MoodleApiIntegration.Models
{
    public class LookupServiceType
    {
        [Key]
        public int ServiceID { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string ServiceDesc { get; set; } = string.Empty;
    }
}
