//using Microsoft.EntityFrameworkCore;
//using MoodleApiIntegration.DB;
//using MoodleApiIntegration.Models;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Threading.Tasks;

//namespace MoodleApiIntegration.Services
//{
//    public class MoodleService
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly HttpClient _client;
//        private readonly string _moodleUrl = "https://lms-test.badyauni.edu.eg/webservice/rest/server.php";
//        private readonly string _moodleToken = "19ea01049645f3284622f067805afc1b";
//        private readonly string _moodleFunction = "core_user_update_users";
//        private readonly string _moodleFormat = "json";

//        public MoodleService(ApplicationDbContext context)
//        {
//            _context = context;
//            _client = new HttpClient();
//        }

//        /// <summary>
//        /// Activates a specific suspended user in Moodle
//        /// </summary>
//        public async Task<bool> ActivateUserAsync(int userId)
//        {
//            var student = _context.StudentStopList.FirstOrDefault(s => s.Status== false && s.StudentID == userId);

//            if (student == null)
//            {
//                return false;
//            }

//            var users = new List<KeyValuePair<string, string>>
//            {
//                new KeyValuePair<string, string>("wstoken", _moodleToken),
//                new KeyValuePair<string, string>("wsfunction", _moodleFunction),
//                new KeyValuePair<string, string>("moodlewsrestformat", _moodleFormat),
//                new KeyValuePair<string, string>("users[0][id]", student.StudentID.ToString()),
//                new KeyValuePair<string, string>("users[0][suspended]", "0")
//            };

//            var content = new FormUrlEncodedContent(users);
//            var response = await _client.PostAsync(_moodleUrl, content);
//            var responseContent = await response.Content.ReadAsStringAsync();

//            if (response.IsSuccessStatusCode)
//            {
//                student.Status =true;
//                _context.SaveChanges();
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        /// <summary>
//        /// Retrieves students who are on hold using the stored procedure `GetOnHoldStudents`
//        /// </summary>
//        public async Task<List<StudentStopList>> GetOnHoldStudentsAsync()
//        {
//            return await _context.StudentStopList
//                .FromSqlRaw("EXEC GetOnHoldStudents")
//                .ToListAsync();
//        }
//    }
//}

using Microsoft.EntityFrameworkCore;
using MoodleApiIntegration.DB;
using MoodleApiIntegration.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoodleApiIntegration.Services
{
    public class MoodleService
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _client;
        private readonly string _moodleUrl = "https://lms-test.badyauni.edu.eg/webservice/rest/server.php";
        private readonly string _moodleToken = "19ea01049645f3284622f067805afc1b";
        private readonly string _moodleFunctionUpdate = "core_user_update_users";
        private readonly string _moodleFunctionGet = "core_user_get_users";
        private readonly string _moodleFormat = "json";

        public MoodleService(ApplicationDbContext context)
        {
            _context = context;
            _client = new HttpClient();
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
        //public async Task<List<StudentStopList>> GetOnHoldStudentsFromProcedureAsync()
        //{
        //    try
        //    {
        //        return await _context.StudentStopList
        //     .FromSqlRaw("EXEC dbo.GetOnHoldStudents")
        //     .ToListAsync();

        //    }
        //    catch (Exception ex)
        //    {
        //        System.Console.WriteLine($"❌ Error executing stored procedure: {ex.Message}");
        //        return new List<StudentStopList>();
        //    }
        //}

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
                return new List<OnHoldStudentDto>(); // Empty list on failure
            }
        }


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
                return new List<ClearedStudentsDto>(); // Empty list on failure
            }
        }


    }
}

