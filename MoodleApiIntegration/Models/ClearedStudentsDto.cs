namespace MoodleApiIntegration.Models
{
    using Microsoft.EntityFrameworkCore;
    using System;

    [Keyless]  // ✅ Mark as keyless because this is a stored procedure result
    public class ClearedStudentsDto
    {
        public int StopListID { get; set; }
        public int StudentID { get; set; }
        public int ReasonID { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? ClearedDate { get; set; }
        public string AddedBy { get; set; }=string.Empty;
        public string Comments { get; set; }=string.Empty;
        public int ServiceID { get; set; }
        public bool IS_SUCCESS { get; set; }
    }

}
