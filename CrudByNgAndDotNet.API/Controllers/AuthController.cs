using CrudByNgAndDotNet.API.Models;
using CrudByNgAndDotNet.API.Models.DTO;
using CrudByNgAndDotNet.API.Repositories.Interface;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using CrudByNgAndDotNet.API.Models.Domain;
using CrudByNgAndDotNet.API.Helper;
using CrudByNgAndDotNet.API.Models.model;
namespace CrudByNgAndDotNet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<RegisterUser> userManager;
        private readonly ITokenRepository tokenRepository;
        private readonly ISendEmail _emailSender;
        public AuthController(UserManager<RegisterUser> userManager,
            ITokenRepository tokenRepository,
            ISendEmail emailSender)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            _emailSender = emailSender;
        }

        // POST: {apibaseurl}/api/auth/login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseHelper.FailureResult("Invalid request", StatusCodes.Status400BadRequest));
            }
            var identityUser = await userManager.FindByEmailAsync(request.Email);
            if (identityUser is not null)
            { 
            var checkPasswordResult = await userManager.CheckPasswordAsync(identityUser, request.Password);
                if (checkPasswordResult)
                {
                    var roles = await userManager.GetRolesAsync(identityUser);
                   // Create a Token and Response
                    var jwtToken = tokenRepository.CreateJwtToken(identityUser, roles.ToList());
                    var response = new LoginResponseDto()
                    {
                        FirstName = identityUser.FirstName,
                        LastName = identityUser.LastName,
                        isRegularUser = identityUser.isRegularUser,
                        Address = identityUser.Address,
                        Email = request.Email,
                        Roles = roles.ToList(),
                        Token = jwtToken
                    };
                    return Ok(ApiResponseHelper.SuccessResult(response, "User Logged in successfully"));
                }
            }
             return Unauthorized(ApiResponseHelper.FailureResult("Invalid email or password.", StatusCodes.Status401Unauthorized));
        }


        // POST: {apibaseurl}/api/auth/register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel request)
        {
            // Create IdentityUser object
            var user = new RegisterUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                UserName = request.Email,
                Email = request.Email,
                Address = request.Address,
                isRegularUser = request.isRegularUser,
                Role = request.Role,
            };
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseHelper.FailureResult("Invalid request.", StatusCodes.Status400BadRequest));
            }
            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(ApiResponseHelper.FailureResult("Invalid request", StatusCodes.Status401Unauthorized, result.Errors));
            }

            // Add user to the specified role
            if (!string.IsNullOrEmpty(request.Role))
            {
                await userManager.AddToRoleAsync(user, request.Role);
            }
            return Ok(ApiResponseHelper.SuccessResult(user , "User registered successfully."));
        }

        // RESET PASSWORD

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponseHelper.FailureResult("Invalid request", StatusCodes.Status400BadRequest));

            var user = await userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
                return NotFound(ApiResponseHelper.FailureResult("User not found.", StatusCodes.Status404NotFound));

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string?>
            {
                {"token", token },
                {"email", forgotPasswordDto.Email }
            };

            var callback = QueryHelpers.AddQueryString(forgotPasswordDto.ClientURI, param);
            var message = new Message(new string[] { user.Email }, "Reset password token", callback, null);

            await _emailSender.ForgotPasswordSendEmailAsync(message);

            return Ok(ApiResponseHelper.SuccessResult(param, "Reset password request submitted successfully."));
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponseHelper.FailureResult("Invalid request", StatusCodes.Status400BadRequest));

            var user = await userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
                return NotFound(ApiResponseHelper.FailureResult("Data not found.", StatusCodes.Status404NotFound));

            var resetPassResult = await userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
            if (!resetPassResult.Succeeded)
            {
                var errors = resetPassResult.Errors.Select(e => e.Description);

                return BadRequest(ApiResponseHelper.FailureResult("Invalid request", StatusCodes.Status400BadRequest,errors));
            }

            return Ok(ApiResponseHelper.SuccessResult("Reset password successfully.", "Password has been changed, Please enter new credential to login."));
        }
    }
}
