using System.ComponentModel.DataAnnotations;

namespace MoodleApiIntegration.Models
{
    public class LookupStopListReason
    {
        [Key]
        public int ReasonID { get; set; }
        public string ReasonDesc { get; set; } = string.Empty;
    }
}
