using Microsoft.AspNetCore.Identity;

namespace CrudByNgAndDotNet.API.Models.model
{
    public class RegisterUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public bool isRegularUser { get; set; }
        public string Role { get; set; }
    }
}
