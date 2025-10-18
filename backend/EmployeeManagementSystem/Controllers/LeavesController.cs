using EmployeeManagementSystem.Dto;
using EmployeeManagementSystem.Helpers;
using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Mappers;
using EmployeeManagementSystem.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    
    public class LeavesController : ControllerBase
    {
        private readonly ILeaveRepository _leaveRepo;
        private readonly IUserContextService userContext;
        private readonly IRepository<Attendance> _attendanceRepo;


        public LeavesController(ILeaveRepository LeaveRepo, IUserContextService userContext, IRepository<Attendance> attendanceRepo)
        {
            _leaveRepo = LeaveRepo;
            this.userContext = userContext;
            attendanceRepo = attendanceRepo;
        }

        [HttpPost("apply")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> ApplyLeave([FromBody] LeaveDto model)
        {
            if (model == null)
                return BadRequest("Invalid leave data.");

            // 🔹 Get EmployeeId from JWT claims via UserContextService
            var employeeId = await userContext.GetEmployeeIdFromClaimsAsync(User);
            if (employeeId == null)
                return Unauthorized("Employee not found.");

            // Use the mapper to create a Leave entity
            var leave = model.ToLeave((int)employeeId);

            // Save it to DB via repository
            await _leaveRepo.AddAsync(leave);
            await _leaveRepo.SaveChangesAsync();

            return Ok(new { message = "Leave applied successfully" });
        }

        [HttpPut("update-leave")]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<IActionResult> UpdateLeaveStatus([FromBody] LeaveDto model)
        {
            if (model == null)
                return BadRequest("Invalid leave data.");

            var leave = await _leaveRepo.GetByIdAsync(model.Id!.Value);
            if (leave == null)
                return NotFound("Leave not found.");

            //  Check role from JWT claim
            var isAdmin = userContext.IsAdmin(User);

            if (isAdmin)
            {
                //  Admin can update any status (use mapper directly)
                leave.UpdateLeaveFromDto(model);

                if (model.Status == (int)LeaveStatus.Accepted)
                {
                    //  If leave is accepted, we mark the leave on attendence as well
                    //If not marked, create new record
                    var attendance = new Attendance
                    {
                        Date = leave.LeaveDate.ToDateTime(TimeOnly.MinValue), //isliye kia q k leavedate DateOnly h jo mene datetime k bajae rkhi th is liye type mismatch sy bachny k liye convert kia
                        EmployeeId = leave.EmployeeId,
                        Type = (int)AttendenceType.Leave
                    };
                    await _attendanceRepo.AddAsync(attendance);

                }
            }
            else
            {
                //  Employee can only cancel their leave
                if (model.Status == (int)LeaveStatus.Cancelled)
                {
                    leave.UpdateLeaveFromDto(model);
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new { message = "Only admin can change this status." });

                }
            }

            _leaveRepo.Update(leave);
            await _leaveRepo.SaveChangesAsync();

            return Ok(new { message = "Leave updated successfully" });
        }

        [HttpGet]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<IActionResult> LeavesList([FromQuery] SearchOptions options)
        {
            //  Check role from JWT claim
            var isAdmin = userContext.IsAdmin(User);

            if (isAdmin)
            {
                var result = await _leaveRepo.GetAllAsync(options);
                return Ok(result);
            }
            else
            {
                var employeeId = await userContext.GetEmployeeIdFromClaimsAsync(User);
                if (employeeId == null)
                    return Unauthorized("Employee not found.");

                var result = await _leaveRepo.GetByEmployeeIdAsync(employeeId.Value, options);
                return Ok(result);

            }




        }


    }
}
