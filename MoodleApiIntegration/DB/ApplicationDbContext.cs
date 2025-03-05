using Microsoft.EntityFrameworkCore;
using MoodleApiIntegration.Models;

namespace MoodleApiIntegration.DB
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<StudentStopList> StudentStopList { get; set; }
    
        public DbSet<LookupStopListReason> LookupStopListReason { get; set; }
        public DbSet<LookupServiceType> LookupServiceType { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Master_job> Master_Jobs { get; set; }
        public DbSet<Job_Log> job_Logs { get; set; }
        public DbSet<OnHoldStudentDto> onHoldStudentDtos { get; set; }
        public DbSet<ClearedStudentsDto> clearedStudentsDtos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LookupServiceType>()
                .HasKey(s => s.ServiceID); // Define ServiceID as the primary key

            base.OnModelCreating(modelBuilder);
        }
    }
}
