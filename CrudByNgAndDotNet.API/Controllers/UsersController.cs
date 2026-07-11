using CrudByNgAndDotNet.API.Helper;
using CrudByNgAndDotNet.API.Models;
using CrudByNgAndDotNet.API.Models.DTO;
using CrudByNgAndDotNet.API.Models.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudByNgAndDotNet.API.Controllers
{
    [Authorize] // Requires a valid JWT bearer token for all endpoints by default
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<RegisterUser> _userManager;

        public UsersController(UserManager<RegisterUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: api/users
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")] // Only Admins or Managers can view the entire registry
        public async Task<IActionResult> GetAllRegisteredUsers()
        {
            // Fully asynchronous database fetch using EF Core projection
            var identityUsers = await _userManager.Users.ToListAsync();

            // Transform domain models to Data Transfer Objects cleanly
            var responseData = identityUsers.Select(user => new UserResponseDTo
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                isRegularUser = user.isRegularUser,
                Role = user.Role,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            }).ToList();

            return Ok(ApiResponseHelper.SuccessResult(responseData, "Registered users registry retrieved successfully."));
        }

        // DELETE: api/users/{id}
        [HttpDelete]
        [Route("{id}")] // Generic string route pattern matching the parameters schema
        [Authorize(Roles = "Admin")] // Critical destructive actions must require top-tier Admin elevation
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            // Verify if the requested user identity profile exists on the host
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(ApiResponseHelper.FailureResult($"User profile with reference identity '{id}' could not be located.", StatusCodes.Status404NotFound));
            }

            // Execute database record eviction asynchronously
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok(ApiResponseHelper.SuccessResult($"User profile assigned to '{user.Email}' has been permanently terminated.", "User account eviction completed successfully."));
            }

            // Extract native identity engine exceptions to stream back to the log schema pipeline
            var structuralErrors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(ApiResponseHelper.FailureResult("Account termination execution rejected by internal constraints.", StatusCodes.Status400BadRequest, structuralErrors));
        }
    }
}
