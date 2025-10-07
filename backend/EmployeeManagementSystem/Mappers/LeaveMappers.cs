using EmployeeManagementSystem.Dto;
using EmployeeManagementSystem.Model;

namespace EmployeeManagementSystem.Mappers
{
    public static class LeaveMappers
    {
        public static Leave ToLeave(this LeaveDto dto)
        {
            return new Leave
            {
                Type = (int)dto.Type,
                Reason = dto.Reason,
                LeaveDate = dto.LeaveDate,
                Status = (int)LeaveStatus.Pending,
                EmployeeId = (int)dto.EmployeeId
            };
        }

    }
}
