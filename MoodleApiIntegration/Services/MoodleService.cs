

using Microsoft.EntityFrameworkCore;
using MoodleApiIntegration.DB;
using MoodleApiIntegration.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using static MoodleApiIntegration.Services.MoodleService;

namespace MoodleApiIntegration.Services
{
    public interface IStudentService
    {
        Task<List<OnHoldStudentDto>> GetOnHoldStudentsFromProcedureAsync();
        Task<List<ClearedStudentsDto>> GetClearedStudentsFromProcedureAsync();
        Task InsertJobLogAsync(List<int> studentIds, string exceptionMessage, bool isSuccess, bool isOnHold);


    }
    public class MoodleService: IStudentService
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _client;
        private readonly string _moodleUrl = "https://lms-test.badyauni.edu.eg/webservice/rest/server.php";
        private readonly string _moodleToken = "19ea01049645f3284622f067805afc1b";
        private readonly string _moodleFunctionUpdate = "core_user_update_users";
        private readonly string _moodleFunctionGet = "core_user_get_users";
        private readonly string _moodleFormat = "json";
        //for helper 
        private readonly IHttpContextAccessor _httpContextAccessor;


        private const int StatusActive = 1;
        private const int StatusHold = 2;
      



        public MoodleService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _client = new HttpClient();
          
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Activates a specific user in Moodle, even if they are not in the local database.
        /// </summary>
        public async Task<bool> ActivateUserAsync(int userId)
        {
            // ✅ Step 1: Check if user exists in Moodle
            bool userExists = await CheckIfUserExistsInMoodle(userId);
            if (!userExists)
            {
                System.Console.WriteLine($"❌ User {userId} not found in Moodle.");
                return false;
            }

            // ✅ Step 2: Prepare the API request to activate the user
            var users = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("wstoken", _moodleToken),
                new KeyValuePair<string, string>("wsfunction", _moodleFunctionUpdate),
                new KeyValuePair<string, string>("moodlewsrestformat", _moodleFormat),
                new KeyValuePair<string, string>("users[0][id]", userId.ToString()),
                new KeyValuePair<string, string>("users[0][suspended]", "0") // Activate user (0 = Active)
            };

            var content = new FormUrlEncodedContent(users);

            // ✅ Step 3: Call Moodle API to activate user
            var response = await _client.PostAsync(_moodleUrl, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            // ✅ Step 4: Check API response
            if (response.IsSuccessStatusCode)
            {
                System.Console.WriteLine($"✅ User {userId} activated successfully in Moodle.");
                return true;
            }
            else
            {
                System.Console.WriteLine($"❌ Failed to activate user {userId} in Moodle. Response: {responseContent}");
                return false;
            }
        }

        /// <summary>
        /// Checks if a user exists in Moodle.
        /// </summary>
        private async Task<bool> CheckIfUserExistsInMoodle(int userId)
        {
            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("wstoken", _moodleToken),
                new KeyValuePair<string, string>("wsfunction", _moodleFunctionGet),
                new KeyValuePair<string, string>("moodlewsrestformat", _moodleFormat),
                new KeyValuePair<string, string>("criteria[0][key]", "id"),
                new KeyValuePair<string, string>("criteria[0][value]", userId.ToString())
            };

            var content = new FormUrlEncodedContent(parameters);
            var response = await _client.PostAsync(_moodleUrl, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            // ✅ Check if the response contains valid user data
            if (response.IsSuccessStatusCode && !responseContent.Contains("\"users\":[]"))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        //        /// Retrieves students who are on hold using the stored procedure `GetOnHoldStudents`
        //        /// </summary>
        /// <summary>
        /// Retrieves a list of students who are on hold from the stored procedure `GetOnHoldStudents`
        /// </summary>


        //Helper method to insert data into Master_job and Job_log tables


        // Helper method to insert data into Master_job and Job_log tables






        public async Task<List<OnHoldStudentDto>> GetOnHoldStudentsFromProcedureAsync()
        {
            try
            {
                return await _context.onHoldStudentDtos
                    .FromSqlRaw("EXEC dbo.GetOnHoldStudents")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"❌ Error executing stored procedure: {ex.Message}");
                return new List<OnHoldStudentDto>(); // Return empty list on failure
            }
        }

        /// <summary>
        /// Retrieves students who are cleared using the stored procedure `GetClearedStudents`.
        /// </summary>
        public async Task<List<ClearedStudentsDto>> GetClearedStudentsFromProcedureAsync()
        {
            try
            {
                return await _context.clearedStudentsDtos
                    .FromSqlRaw("EXEC dbo.GetClearedStudents")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"❌ Error executing stored procedure: {ex.Message}");
                return new List<ClearedStudentsDto>(); // Return empty list on failure
            }
        }

        /// <summary>
        /// Inserts data into the Master_job and Job_log tables.
        /// </summary>
        public async Task InsertJobLogAsync(List<int> studentIds, string exceptionMessage, bool isSuccess, bool isOnHold)
        {
            try
            {
                // Get the server IP address
                var serverIp = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "Unknown";

                // Determine the status based on isSuccess
                int currentStatus = isOnHold ? StatusHold : StatusActive;

                // Insert into Master_job table
                var masterJob = new Master_job
                {
                    date_run = DateTime.UtcNow,
                    Current_status = currentStatus, // Use integer values (1 = Active, 2 = Hold)
                    ServerIP = serverIp
                };
                _context.Master_Jobs.Add(masterJob);
                await _context.SaveChangesAsync();

                // Insert into Job_log table for each student ID
                foreach (var studentId in studentIds)
                {
                    var jobLog = new Job_Log
                    {
                        Master_ID = masterJob.Master_ID,
                        student_id = studentId,
                        exception_message = exceptionMessage,
                        IS_SUCCESS = isSuccess
                    };
                    _context.job_Logs.Add(jobLog);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the error (you can use a logging framework like Serilog or NLog)
                System.Console.WriteLine($"❌ Error inserting job log: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }
    }
}
