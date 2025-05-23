﻿using Core;
using Core.Identity.Entities;
using Core.Interface;
using FitGuide.DTOs;
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

    public class InjuryController :BaseAPI
    {
        private readonly FitGuideContext _fitGuideContext;
        private readonly UserManager<User> _userManager;
        private readonly IGeneric<Injury> _repoInjury;
        private readonly IGeneric<UserInjury> _repoUserInjury;

        public InjuryController(FitGuideContext fitGuideContext,UserManager<User> userManager,IGeneric<Injury> repoInjury,IGeneric<UserInjury> repoUserInjury)
        {
            _fitGuideContext = fitGuideContext;
            _userManager = userManager;
            _repoInjury = repoInjury;
            _repoUserInjury = repoUserInjury;
        }
        [HttpGet("GetAllInjuries")]
        public async Task<ActionResult> GetInjuries()
        {
            var goals = await _repoInjury.GetAllAsync();
            var goalName = goals.Select(g => g.Name).ToList();
            return Ok(goalName);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("AddInjury")]
        public async Task<ActionResult<InjuryUserDTO>> AddInjury(UserInjuryDTO userInjury)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "User UnAuthorized" } });
            }

            var injuries = await _repoInjury.GetAllAsync();
            var addedinjury = new List<string>();
            if (userInjury == null) { return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "Injury is not supported or available" } }); }
            var exisitinginjury = await _repoUserInjury.GetFirstAsync(u => u.UserId.Equals(user.Id) && u.injuryId == userInjury.Id);
            if (exisitinginjury != null)
            {
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "Injury already added" } });
            }
                    var newuser = new UserInjury
                    {
                        UserId = user.Id,
                        injuryId = userInjury.Id,
                    };
                    await _repoUserInjury.AddAsync(newuser);
                    addedinjury.Add(newuser.injury.Name);
            //var mapper = _mapper.Map<InjuryUserDTO>(exisitinginjury);
            //mapper.UserId = user.Id;
            //var injuryuser = _mapper.Map<UserInjury>(mapper);

            return Ok($"{newuser.injury.Name} has been added");




        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("GetUserInjuries")]
        public  async Task<ActionResult> GetAllInjuries()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "User UnAuthorized" } });
            }
            var injuries =await _fitGuideContext.userInjuries.Where(u => u.UserId == user.Id).Include(i => i.injury).ToListAsync();
            if (!injuries.Any()) { return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { $"No Injuries related to {user.FistName}" } }); }
            var userInjuries = injuries.GroupBy(u => u.UserId).Select(u => new
            {
                Description = $"Injuries for {user.FistName}",
                injuries = injuries.Select(u=>u.injury.Name),
            }).ToList();
            return Ok(userInjuries);
        }


    }
}
