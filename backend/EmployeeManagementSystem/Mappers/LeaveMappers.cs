using EmployeeManagementSystem.Dto;
using EmployeeManagementSystem.Model;

namespace EmployeeManagementSystem.Mappers
{
    public static class LeaveMappers
    {
        public static Leave ToLeave(this LeaveDto dto, int empId)
        {
            return new Leave
            {
                Type = (int)dto.Type,
                Reason = dto.Reason!,
                LeaveDate = dto.LeaveDate.Value,
                Status = (int)LeaveStatus.Pending,
                EmployeeId = empId
            };
        }

        public static void UpdateLeaveFromDto(this Leave leave, LeaveDto dto)
        {
            leave.Status = dto.Status!.Value;


        }

    }
}
