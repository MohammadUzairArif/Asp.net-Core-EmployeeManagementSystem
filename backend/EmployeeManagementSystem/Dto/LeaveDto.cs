namespace EmployeeManagementSystem.Dto
{
    public class LeaveDto
    {
        public int? Id { get; set; }
        public int? Type { get; set; }
        public string Reason { get; set; }
        public int? Status { get; set; }
        public DateOnly LeaveDate { get; set; }
        public int? EmployeeId { get; set; }
    }
}
