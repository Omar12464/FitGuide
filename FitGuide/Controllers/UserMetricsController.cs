using AutoMapper;
using Core;
using Core.Identity.Entities;
using Core.Interface;
using Core.Interface.Services;
using FitGuide.DTOs;
using FitGuide.ErrorsManaged;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository;
using ServiceLayer;
using System.Net.Http.Headers;

namespace FitGuide.Controllers
{

    public class UserMetricsController : BaseAPI
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IGeneric<NutritionPlan> _repoNutritionPlan;
        private readonly IGeneric<UserMetrics> _repo;
        private readonly FitGuideContext _fitGuideContext;
        private readonly IWeightCategory _weightCategory;
        private readonly IWeightTarget _weightTarget;
        private readonly IUserMetricsServices _userMetrics;

        public UserMetricsController(UserManager<User> userManager, IMapper mapper,IGeneric<NutritionPlan>repoNutritionPlan, IUserMetricsServices userMetrics, IGeneric<UserMetrics> Repo,FitGuideContext fitGuideContext,IWeightCategory weightCategory, IWeightTarget weightTarget)
        {
            _userManager = userManager;
            _mapper = mapper;
            _repoNutritionPlan = repoNutritionPlan;
            _repo = Repo;
            _fitGuideContext = fitGuideContext;
            _weightCategory = weightCategory;
            _weightTarget = weightTarget;
            _userMetrics = userMetrics;
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("EnterMetrics")]
        public async Task<ActionResult> AddMetrics(UserMetricsDTO userMetrics)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "User UnAuthorized" } });
            }
            if (userMetrics == null) { return BadRequest(new ApiExceptionResponse(400)); }
            var bmi = _userMetrics.CalculateBMI(userMetrics.Weight, userMetrics.Height);
            var weightcat = _weightCategory.GetUserWeightCategory(bmi, userMetrics.Fat);

            var Metrics = _mapper.Map<UserMetricsDTO, UserMetrics>(userMetrics);
            Metrics.UserId = user.Id;
            Metrics.BMI = bmi;
            Metrics.CreatedAt = DateTime.UtcNow;
            Metrics.weightCategory = weightcat;
            //bool exist = await _userMetrics.CheckMetrics(Metrics.UserId);
            //if (exist is true) { return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "This Metrics exist" } }); }
            await _repo.AddAsync(Metrics);
            return Ok($"{user.FistName} metrics has been added successfully");
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("UpdateMetrics")]
        public async Task<ActionResult> UpdateMetrics(UpdateUserMetricsDTO userMetrics)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "User UnAuthorized" } });
            }
            if (userMetrics == null) { return BadRequest(new ApiExceptionResponse(400)); }

            var existedmetricsdata = await _repo.GetAllAsync();
            var existedmetrics = existedmetricsdata.OrderByDescending(u => u.CreatedAt).FirstOrDefault(us=>us.UserId==user.Id);
            existedmetrics.Height = userMetrics.Height ?? existedmetrics.Height;
            existedmetrics.Weight = userMetrics.Weight ?? existedmetrics.Weight;
            existedmetrics.MuscleMass = userMetrics.MuscleMass ?? existedmetrics?.MuscleMass;
            existedmetrics.WaterMass = userMetrics.WaterMass ?? existedmetrics?.WaterMass;
            existedmetrics.fitnessLevel = userMetrics.fitnessLevel ?? existedmetrics.fitnessLevel;
            existedmetrics.BMI = _userMetrics.CalculateBMI(existedmetrics.Weight, existedmetrics.Height);
            existedmetrics.weightCategory = _weightCategory.GetUserWeightCategory(existedmetrics.BMI??0, existedmetrics.Fat??0);
            existedmetrics.GymFrequency = userMetrics.GymFrequency;

            try
            {
                _repo.UpdateAsync(existedmetrics);

                
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiExceptionResponse(500, "An error occurred while updating metrics.", ex.Message));
            }

            return Ok($"{user.FistName} metrics has been updated successfully");
        }
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("GetAllUserMetrices")]
        public async Task<ActionResult<UserMetricsDTO>> GetMetrcies()
        {
            var user=await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "User UnAuthorized" } });
            }
            var userMetricsData = await _repo.GetAllAsync();
            var userMetrics = userMetricsData.Where(u => u.UserId.Equals(user.Id)).ToList();


            var allMetrics = userMetrics.Select(group=>new
            {
                User = $"Metrices related to {user.FistName}",
                UserMetrics = userMetrics.Select(metric => new
                { MetricsId=metric.Id,
                  CurrentBBMI=metric.BMI,
                  Weight=metric.Weight,
                  Height=metric.Height,
                  Fat=metric.Fat,
                  MuscleMass=metric.MuscleMass,
                  WaterMass = metric.WaterMass,
                  weightCategory = metric.weightCategory.ToString(),
                  fitnessLevel=metric.fitnessLevel.ToString(),
                  GymFrequency = metric.GymFrequency,
                }).ToList()

            });
            return Ok(allMetrics);

        }

    }
}
