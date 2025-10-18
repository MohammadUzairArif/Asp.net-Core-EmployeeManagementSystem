using EmployeeManagementSystem.Helpers;
using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceRepository _attendanceRepo;
        private readonly IUserContextService _userService;
        public AttendanceController(IAttendanceRepository attendanceRepo, IUserContextService userService)
        {
            _attendanceRepo = attendanceRepo;
            _userService = userService;
        }

        [HttpPost("mark-present")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> MarkAttendence()
        {
            var employeeId = await _userService.GetEmployeeIdFromClaimsAsync(User);

            if (employeeId == null)
                return Unauthorized("Employee not found");
            //  Check if already marked for today
            var attendenceList = await _attendanceRepo.FindAsync(
                x => x.EmployeeId == employeeId.Value &&
                     DateTime.Compare(x.Date.Date, DateTime.UtcNow.Date) == 0
            );

            if (attendenceList != null)
            {
                return BadRequest(new { message = "Already marked present for today." });

            }


            //If not marked, create new record
            var attendence = new Attendance
            {
                Date = DateTime.UtcNow,
                EmployeeId = employeeId.Value,
                Type = (int)AttendenceType.Present
            };

            await _attendanceRepo.AddAsync(attendence);
            await _attendanceRepo.SaveChangesAsync();

            return Ok(new { message = "Attendance marked as present successfully." });

        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAttendanceHistory([FromQuery] SearchOptions options)
        {
            // If not admin, get employee ID from token/user context
            if (!_userService.IsAdmin(User))
            {
                options.EmployeeId = await _userService.GetEmployeeIdFromClaimsAsync(User);
            }

            var result = await _attendanceRepo.GetAttendanceHistoryAsync(options);
            return Ok(result);
        }

    }
}
