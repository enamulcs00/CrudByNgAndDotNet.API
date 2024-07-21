using CrudByNgAndDotNet.API.Models;
using CrudByNgAndDotNet.API.Models.Domain;
using CrudByNgAndDotNet.API.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudByNgAndDotNet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<RegisterRequestDto> userManager;
        public UsersController(UserManager<RegisterRequestDto> userManager)
        {
            this.userManager = userManager;
        }


        [HttpGet]
        public IActionResult GetAllRegisteredUsers()
        {
            var identityUser = userManager.Users;
            // Convert Domain model to DTO
            var response = new List<UserResponseDTo>();
            foreach (var user in identityUser)
            {
                response.Add(new UserResponseDTo
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Address = user.Address,
                    isRegularUser = user.isRegularUser,
                    Role = user.Role,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                });
            }
            return Ok(response);
        }
    }
}
