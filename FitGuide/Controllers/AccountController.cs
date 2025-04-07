using Core.Identity;
using Core.Identity.Entities;
using Core.Identity.Interfaces;
using FitGuide.DTOs;
using FitGuide.ErrorsManaged;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace FitGuide.Controllers
{
    public class AccountController:BaseAPI
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signIn;
        private readonly IAuthService _authService;

        public AccountController(UserManager<User> userManager,SignInManager<User> signIn,IAuthService authService)
        {
            _userManager = userManager;
            _signIn = signIn;
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO register)
        {
            if ((await CheckEmailExist(register.Email)).Value)
            {
                return BadRequest(new ApiValidationErrorResponse() { Errors= new string[] {"Email Exist"} });
            }
            var user = new User()
            {
                FistName = register.FirstName,
                LastName = register.LastName,
                UserName = register.Email.Split("@")[0],
                Email = register.Email,
                Gender = register.Gender,
                Age = register.Age,
                Country = register.Country,
                PhoneNumber = register.PhoneNumber,
            };
            var RegisteredUser=await _userManager.CreateAsync(user,register.Password);
            if (RegisteredUser.Succeeded is false) { return BadRequest(new APIResponse(400)); }
            else
            {
                await _signIn.SignInAsync(user,isPersistent: true);
                return Ok(new UserDTO
                {
                    FullName = register.FullName,
                    Email = register.Email,
                    Password=register.Password,
                    Token = await _authService.CreateTokenAsync(user, _userManager),
                    CreatedAt = DateTimeOffset.UtcNow

                });
            }
        }

        [HttpPost("LogOut")]
        public async Task<ActionResult> Logout()
        {
            await _signIn.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        [HttpGet("emailExist")]

        public async Task<ActionResult<bool>> CheckEmailExist(string email) 
        {
            return await _userManager.FindByEmailAsync(email.ToLower()) is not null;
        
        }

    }
}
