using Microsoft.AspNetCore.Identity;

namespace CrudByNgAndDotNet.API.Models.DTO
{
    public class RegisterRequestDto : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public bool isRegularUser { get; set; }
        public string Role { get; set; }
    }
}
