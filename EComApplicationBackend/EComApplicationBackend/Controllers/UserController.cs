using App.Core.App.User.Command;
using App.Core.App.User.Query;
using App.Core.Models;
using MediatR;
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
        public async Task<IActionResult> RegisterUser(RegistrationDto registrationDto)
        {
            var result = await _mediator.Send(new CreateUserCommand { Registration = registrationDto });
            if(result == null)
            {
                return BadRequest("User Already Exists");
            }
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser(LoginDto loginDto)
        {
            var result = await _mediator.Send(new LoginUserCommand { Login = loginDto });
            if(result == null)
            {
                return BadRequest();
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

        [HttpGet("Roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _mediator.Send(new GetUserRoleQuery());
            return Ok(result);
        }
    }
}
