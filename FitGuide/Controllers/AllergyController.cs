using Core;
using Core.Identity.Entities;
using Core.Interface;
using FitGuide.ErrorsManaged;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace FitGuide.Controllers
{

    public class AllergyController : BaseAPI
    {
        private readonly UserManager<User> _userManager;
        private readonly IGeneric<Allergy> _repoAllergy;
        private readonly FitGuideContext _fitGuideContext;

        public AllergyController(UserManager<User> userManager,IGeneric<Allergy> repoAllergy,FitGuideContext fitGuideContext)
        {
            _userManager = userManager;
            _repoAllergy = repoAllergy;
            _fitGuideContext = fitGuideContext;
        }
        [HttpGet("Show All Allergies")]
        public async Task<ActionResult> GetAllAllergies()
        {
            var goals = await _repoAllergy.GetAllAsync();
            var goalName = goals.Select(g => g.Name).ToList();
           
            return Ok(goalName);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("GetAllergies")]
        public async Task<ActionResult> GetAllergies()
        {
            var user = await _userManager.GetUserAsync(User);
            var allergies = await _fitGuideContext.userAllergies.Where(u=>u.UserId.Equals(user.Id)).Include(u=>u.allergy).ToListAsync();
            return Ok(allergies);
        }

        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("AddAllergy")]
        public async Task<ActionResult> AddAllergy(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "User UnAuthorized" } });
            }
            var allergy = await _repoAllergy.GetFirstAsync(u => u.Id == id);
            if (allergy == null) {
                return Ok($"User doesn;t have an allergy");
            }
            var userallergy = new UserAllergy()
            {
                UserId = user.Id,
                AllergyId = allergy.Id,
            };
            await _repoAllergy.AddAsync(allergy);
            return Ok($"{allergy.Name} has been added");


        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("ViewMyAllergies")]
        public async Task<ActionResult> ViewMyAllergies()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "User UnAuthorized" } });
            }
            var userallergies =await _repoAllergy.GetAllAsync();
            var userall = userallergies.Where(u => user.Id.Equals(u.Id)).ToList();
            return Ok(userall);
        }
    }
}
