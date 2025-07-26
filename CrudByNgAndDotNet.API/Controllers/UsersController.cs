using CrudByNgAndDotNet.API.Helper;
using CrudByNgAndDotNet.API.Models;
using CrudByNgAndDotNet.API.Models.Domain;
using CrudByNgAndDotNet.API.Models.DTO;
using CrudByNgAndDotNet.API.Repositories.Implementation;
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
            return Ok(ApiResponseHelper.SuccessResult(response));
        }
        // DELETE: {apibaseurl}/api/blogposts/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null) 
                return NotFound();

            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
                return NoContent();  // Return 204 No Content for a successful delete.

            return StatusCode(500, "Internal server error");
        }
    }
}
