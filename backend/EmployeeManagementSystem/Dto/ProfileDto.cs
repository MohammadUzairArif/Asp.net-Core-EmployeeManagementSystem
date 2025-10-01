namespace EmployeeManagementSystem.Dto
{
    public class ProfileDto
    {
        public string UserId { get; set; }
        public string Name { get; set; }
       
        public string Phone { get; set; }
        public string ProfileImage { get; set; }
        
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
