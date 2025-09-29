using Microsoft.AspNetCore.Identity;

namespace EmployeeManagementSystem.Model
{
    public class User : IdentityUser
    {
       public string ProfileImage { get; set; }
    }
}
