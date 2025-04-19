using Core.Identity;
using Core.Identity.Entities;
using Core.Identity.Interfaces;
using FitGuide.DTOs;
using FitGuide.ErrorsManaged;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace FitGuide.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : BaseAPI
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signIn;
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signIn,
            IAuthService authService,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signIn = signIn;
            _authService = authService;
            _emailService = emailService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegisterDTO register)
        {
            if (register == null)
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "Invalid registration data" } });

            var emailExists = await _userManager.FindByEmailAsync(register.Email.ToLower());
            if (emailExists != null)
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "Email already exists" } });

            var user = new User
            {
                FistName = register.FirstName,
                LastName = register.LastName,
                UserName = register.Email.Split("@")[0],
                Email = register.Email.ToLower(),
                Gender = register.Gender,
                Age = register.Age,
                Country = register.Country,
                PhoneNumber = register.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
                return BadRequest(new ApiValidationErrorResponse { Errors = result.Errors.Select(e => e.Description).ToArray() });

            // Generate email confirmation token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = HttpUtility.UrlEncode(token);
            var confirmationLink = $"{Request.Scheme}://{Request.Host}/confirm-email?userId={user.Id}&token={encodedToken}";

            // Send email
            await _emailService.SendEmailConfirmationAsync(user.Email, confirmationLink);

            return Ok(new
            {
                message = "Registration successful. Please check your email to confirm your account."
            });
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "Invalid confirmation link." } });

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "User not found." } });

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
                return BadRequest(new ApiValidationErrorResponse { Errors = result.Errors.Select(e => e.Description).ToArray() });

            return Ok(new { message = "Email confirmed successfully. You can now log in." });
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
        {
            if (loginDto == null)
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "Invalid login data" } });

            var user = await _userManager.FindByEmailAsync(loginDto.Email.ToLower());
            if (user == null)
                return Unauthorized(new ApiValidationErrorResponse { Errors = new[] { "Invalid email" } });

            if (!user.EmailConfirmed)
                return Unauthorized(new ApiValidationErrorResponse { Errors = new[] { "Email not confirmed. Please check your inbox." } });

            var result = await _signIn.PasswordSignInAsync(user, loginDto.Password, loginDto.RememberMe, false);

            if (!result.Succeeded)
                return Unauthorized(new ApiValidationErrorResponse { Errors = new[] { "Invalid password" } });

            return Ok(new UserDTO
            {
                FullName = $"{user.FistName} {user.LastName}",
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager),
                CreatedAt = DateTimeOffset.UtcNow
            });
        }

        [HttpPost("ResendEmailConfirmation")]
        public async Task<IActionResult> ResendEmailConfirmation([FromBody] string email)
        {
            var user = await _userManager.FindByEmailAsync(email.ToLower());
            if (user == null) return Ok(); // Don't reveal existence

            if (user.EmailConfirmed)
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "Email already confirmed" } });

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = HttpUtility.UrlEncode(token);
            var confirmationLink = $"{Request.Scheme}://{Request.Host}/confirm-email?userId={user.Id}&token={encodedToken}";

            await _emailService.SendEmailConfirmationAsync(user.Email, confirmationLink);

            return Ok(new { message = "Confirmation email resent." });
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO forgotPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email.ToLower());
            if (user == null) return Ok(); // Don't reveal that the user does not exist

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = HttpUtility.UrlEncode(token);
            var resetLink = $"{Request.Scheme}://{Request.Host}/reset-password?email={forgotPasswordDto.Email}&token={encodedToken}";

            if (user.Email != null)
            {
                await _emailService.SendPasswordResetEmailAsync(user.Email, resetLink);
            }

            return Ok(new { message = "If your email is registered, you will receive a password reset link." });
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email.ToLower());
            if (user == null) return Ok(); // Don't reveal that the user does not exist

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = result.Errors.Select(e => e.Description).ToArray() });
            }

            return Ok(new { message = "Password has been reset successfully." });
        }

        [HttpPost("Logout")]
        public async Task<ActionResult> Logout()
        {
            await _signIn.SignOutAsync();
            HttpContext.Session.Clear();
            return Ok(new { message = "Logged out successfully." });
        }
    }
}
