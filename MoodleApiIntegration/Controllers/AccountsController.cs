//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;
//using MoodleApiIntegration.Services;
//using System.Collections.Generic;
//using MoodleApiIntegration.Models;

//[ApiController]
//[Route("api/[controller]")]
//public class AccountsController : ControllerBase
//{
//    private readonly MoodleService _moodleService;

//    public AccountsController(MoodleService moodleService)
//    {
//        _moodleService = moodleService;
//    }

//    /// <summary>
//    /// Activates a suspended user by ID.
//    /// </summary>
//    /// <param name="userId">Moodle User ID to activate</param>
//    /// <returns>Activation status</returns>
//    [HttpPost("activate/{userId}")]
//    public async Task<IActionResult> ActivateUser(int userId)
//    {
//        bool success = await _moodleService.ActivateUserAsync(userId);

//        if (success)
//            return Ok(new { message = $"✅ User {userId} activated successfully." });
//        else
//            return NotFound(new { message = $"❌ No suspended user found with ID {userId}." });
//    }

//    /// <summary>
//    /// Retrieves a list of students who are on hold from the database.
//    /// </summary>
//    /// <returns>List of on-hold students</returns>
//    [HttpGet("onhold")]
//    public async Task<IActionResult> GetOnHoldStudents()
//    {
//        List<StudentStopList> students = await _moodleService.GetOnHoldStudentsAsync();

//        if (students.Count > 0)
//            return Ok(students);
//        else
//            return NotFound(new { message = "❌ No students on hold found." });
//    }
//}
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MoodleApiIntegration.Services;
using MoodleApiIntegration.Models;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly MoodleService _moodleService;

    public AccountsController(MoodleService moodleService)
    {
        _moodleService = moodleService;
    }

    /// <summary>
    /// Activates a suspended user in Moodle (even if not in local DB).
    /// </summary>
    /// <param name="userId">Moodle User ID to activate</param>
    /// <returns>Activation status</returns>
    [HttpPost("activate/{userId}")]
    public async Task<IActionResult> ActivateUser(int userId)
    {
        bool success = await _moodleService.ActivateUserAsync(userId);

        if (success)
            return Ok(new { message = $"✅ User {userId} activated successfully in Moodle." });
        else
            return NotFound(new { message = $"❌ User {userId} not found in Moodle or activation failed." });
    }

    [HttpGet("onhold-procedure")]
    public async Task<IActionResult> GetOnHoldStudentsFromProcedure()
    {
        List<OnHoldStudentDto> studentsDto = await _moodleService.GetOnHoldStudentsFromProcedureAsync();
        List<StudentStopList> students = studentsDto.Select(dto => new StudentStopList
        {
            StopListID = dto.StopListID,
            StudentID = dto.StudentID,
            ReasonID = dto.ReasonID,
          
            DateAdded = dto.DateAdded,
            ClearedDate = dto.ClearedDate,
            AddedBy = dto.AddedBy,
            Comments = dto.Comments
        }).ToList();

        if (students.Count > 0)
            return Ok(students);
        else
            return NotFound(new { message = "❌ No students on hold found in stored procedure." });
    }

    [HttpPost("Active Student")]
    public async Task<IActionResult> GetActiveStudentsFromProcedure()
    {
        List<ClearedStudentsDto> studentsDto = await _moodleService.GetClearedStudentsFromProcedureAsync();
        List<StudentStopList> students = studentsDto.Select(dto => new StudentStopList
        {
            StopListID = dto.StopListID,
            StudentID = dto.StudentID,
            ReasonID = dto.ReasonID,
            DateAdded = dto.DateAdded,
            ClearedDate = dto.ClearedDate,
            AddedBy = dto.AddedBy,
            Comments = dto.Comments
        }).ToList();
        if (students.Count > 0)
            return Ok(students);
        else
            return NotFound(new { message = "❌ No students on hold found in stored procedure." });
    }
}

