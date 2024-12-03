using App.Core.App.Product.Command;
using App.Core.App.User.Command;
using App.Core.App.User.Query;
using App.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EComApplicationBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromForm] RegistrationDto registrationDto)
        {
            var result = await _mediator.Send(new CreateUserCommand { Registration = registrationDto });
            if (result == null)
            {
                return BadRequest("User Already Exists");
            }
            return Ok(result);  
        }

        [HttpPut("Update-User/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] RegistrationDto RegistrationDto)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid User ID.");
            }

            if (RegistrationDto == null)
            {
                return BadRequest("User data cannot be null.");
            }

            var result = await _mediator.Send(new UpdateUserCommand
            {
                UserId = id,
                Registration = RegistrationDto
            });

            if (result == null)
            {
                return NotFound("User not found or update failed.");
            }

            return Ok(result);
        }


        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser(LoginDto loginDto)
        {
            var result = await _mediator.Send(new LoginUserCommand { Login = loginDto });
            if(result == null)
            {
                return Ok("Failure");
            }
            return Ok(result);
        }

        [HttpPost("VerifyOtp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpDto verifyOtpDto)
        {
            var result = await _mediator.Send(new VerifyOtpCommand { VerifyOtp = verifyOtpDto });
            if (result == null)
            {
                return BadRequest("Invalid OTP");
            }
            return Ok(new { Token = result.Token });
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            var result = await _mediator.Send(new ForgotPasswordCommand { ForgotPassword = forgotPasswordDto });
            if (!result)
            {
                return BadRequest("Invalid email address.");
            }
            return Ok("A new password has been sent to your email.");
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            // Assuming you get the UserId from the JWT token or session
            var userId = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }

            var result = await _mediator.Send(new ChangePasswordCommand
            {
                UserId = userId,
                ChangePasswordDto = changePasswordDto
            });

            if (!result)
            {
                return BadRequest("New password cannot be the same as the current password.");
            }

            return Ok("Password changed successfully");
        }

        [HttpGet("Roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _mediator.Send(new GetUserRoleQuery());
            return Ok(result);
        }

        [HttpGet("UserByUsername")]
        public async Task<IActionResult> GetUserByUsername(string UserName)
        {
            var result = await _mediator.Send(new GetUserByUsername { UserName = UserName });
            return Ok(result);
        }
    }
}
