using AutoMapper;
using Core;
using Core.Identity;
using Core.Identity.Entities;
using Core.Identity.Interfaces;
using Core.Interface;
using Core.Interface.Services;
using FitGuide.DTOs;
using FitGuide.ErrorsManaged;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Repository;

namespace FitGuide.Controllers
{
    public class AccountController:BaseAPI
    {
        private readonly FitGuideContext _fitGuideContext;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signIn;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly IUserMetricsServices _userMetrics;
        private readonly IGeneric<UserMetrics> _repo;
        private readonly IGeneric<UserGoal> _repoGoal;
        private readonly IGeneric<GoalTempelate> _repoGoalTemplate;

        public AccountController(FitGuideContext fitGuideContext,UserManager<User> userManager,SignInManager<User> signIn,IAuthService authService,IMapper mapper,IUserMetricsServices userMetrics,IGeneric<UserMetrics> Repo,IGeneric<UserGoal> RepoGoal,IGeneric<GoalTempelate>RepoGoalTemplate)
        {
            _fitGuideContext = fitGuideContext;
            _userManager = userManager;
            _signIn = signIn;
            _authService = authService;
            _mapper = mapper;
            _userMetrics = userMetrics;
            _repo = Repo;
            _repoGoal = RepoGoal;
            _repoGoalTemplate = RepoGoalTemplate;
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

        [HttpPost("LogIn")]
        public async Task<ActionResult<UserDTO>> LogIn(LogInDTO logIn)
        {
            var user = await _userManager.FindByEmailAsync(logIn.EmailAddress);
            if (user== null)
            {
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "Email Doesn't Exist" } });
            }
            else
            {
                var result =await _signIn.CheckPasswordSignInAsync(user, logIn.Password,false);
                if (result.Succeeded is false)
                {
                    return Unauthorized(new APIResponse(401));
                }
                else
                {
                    return Ok(new UserDTO()
                    {
                        FullName=user.FullName,
                        Email = logIn.EmailAddress,
                        Password = logIn.Password,
                        Token = await _authService.CreateTokenAsync(user,_userManager)
                    
                    });
                }
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
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("EnterMetrics")]
        public async Task<ActionResult> AddMetrics(UserMetricsDTO userMetrics)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user==null)
            {
             return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "User UnAuthorized" } });
            }
            if (userMetrics == null) { return BadRequest(new ApiExceptionResponse(400)); }
            var bmi = _userMetrics.CalculateBMI(userMetrics.Weight, userMetrics.Height);
            
            var Metrics= _mapper.Map<UserMetricsDTO,UserMetrics>(userMetrics);
            Metrics.UserId=user.Id;
            Metrics.BMI = bmi;
            Metrics.CreatedAt = DateTime.UtcNow;
            await _repo.AddAsync(Metrics);
            return Ok(Metrics);

        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("UpdateMetrics")]
        public async Task<ActionResult> UpdateMetrics(UpdateUserMetricsDTO userMetrics)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "User UnAuthorized" } });
            }
            if (userMetrics == null) { return BadRequest(new ApiExceptionResponse(400)); }
            
            var existedmetrics = await _repo.GetFirstAsync(ua => ua.UserId == user.Id);
            existedmetrics.Height=userMetrics.Height??existedmetrics.Height;
            existedmetrics.Weight=userMetrics.Weight??existedmetrics.Weight;
            existedmetrics.MuscleMass=userMetrics.MuscleMass??existedmetrics?.MuscleMass;
            existedmetrics.WaterMass=userMetrics.WaterMass??existedmetrics?.WaterMass;
            existedmetrics.Fat=userMetrics.Fat??existedmetrics.Fat;
            existedmetrics.BMI = _userMetrics.CalculateBMI(existedmetrics.Weight, existedmetrics.Height);

            try
            {
                 _repo.UpdateAsync(existedmetrics);
            }
            catch (Exception ex)
            {
                    return StatusCode(500,new ApiExceptionResponse( 500,"An error occurred while updating metrics.",ex.Message));
            }
            
            return Ok(existedmetrics);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("SelectGoal")]

        public async Task<ActionResult> AddGoal(string GoalName)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "User UnAuthorized" } });
            }
            if (GoalName == null) { return BadRequest(new ApiExceptionResponse(400)); }
            var goaltemplate = await _repoGoalTemplate.GetFirstAsync(g => GoalName == g.name);
            if (goaltemplate == null)
            {
                return NotFound(new ApiExceptionResponse(404, "Goal template not found."));
            }
            var usergoal = new UserGoal()
            {
                UserId = user.Id,
                GoalTemplateId = goaltemplate.Id,
                CreatedAt = DateTime.UtcNow,
            };
            await _repoGoal.AddAsync(usergoal);
            var mapper = _mapper.Map<UserGoalDTO>(usergoal);
            return Ok(usergoal);

        }


    }
}

