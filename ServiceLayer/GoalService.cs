//using Core;
//using Core.Identity.Entities;
//using Core.Interface;
//using Core.Interface.Services;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Repository;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ServiceLayer
//{
//    public class GoalService : IGoalServices
//    {
//        private readonly FitGuideContext _fitGuideContext;
//        private readonly IUserMetricsServices _userMetricsServices;
//        private readonly IGeneric<UserMetrics> _repoMetrics;
//        private readonly UserManager<User> _userManager;
//        private readonly IGeneric<GoalTempelate> _repoGoal;
//        private readonly IGeneric<UserGoal> _userGoalRepo;

//        public GoalService(FitGuideContext fitGuideContext,IUserMetricsServices userMetricsServices,IGeneric<UserMetrics> repoMetrics,UserManager<User> userManager,IGeneric<GoalTempelate> repoGoal,IGeneric<UserGoal> userGoalRepo)
//        {
//            _fitGuideContext = fitGuideContext;
//            _userMetricsServices = userMetricsServices;
//            _repoMetrics = repoMetrics;
//            _userManager = userManager;
//            _repoGoal = repoGoal;
//            _userGoalRepo = userGoalRepo;
//        }
//        public async Task<ActionResult> UpdateGoal(string userId,float newGoalName, float TargetWeight, float TargetWaterMass,float TargeMuscleMass)
//        {
//            var user = await _userManager.FindByIdAsync(userId);
//            if (user == null)
//            {
//                throw new Exception("User not found");
//            }
//            var userGoal=await _fitGuideContext.userGoals
//                           .Include(ug => ug.GoalTempelate)
//                           .FirstOrDefaultAsync(ug => ug.UserId == userId);
//            if (userGoal == null)
//            {
//                throw new Exception("Usergoal not found");
//            }
//            if (!string.IsNullOrEmpty(newGoalName))
//            {
//                var newGoalTemplate = await _repoGoal.GetFirstAsync(u => u.name.Equals(newGoalName));
//                if (newGoalTemplate == null)
//                {
//                    throw new Exception("new goal not found");

//                }
//                var newUserGoal = new UserGoal
//                {
//                    UserId = userId,
//                    GoalTemplateId = newGoalTemplate.Id,
//                    CreatedAt = DateTime.Now,
//                    GoalTempelate = newGoalTemplate,
//                };
//                 _userGoalRepo.DeleteAsync(userGoal);
//                var userMetrics = await _repoMetrics.GetFirstAsync(u => u.UserId.Equals(user.Id));
//                var userheight = userMetrics.Height;
//                var bmi = _userMetricsServices.CalculateBMI(TargetWeight, userheight);
//                if (TargetWeight!=0 )
//                {
//                    newUserGoal.GoalTempelate.targetWeight = newUserGoal.GoalTempelate.targetWeight;
//                }

//                if (bmi != 0)
//                {
//                    newUserGoal.GoalTempelate.targetBMI = newUserGoal.GoalTempelate.targetBMI;
//                }

//                if (TargeMuscleMass != 0)
//                {
//                    newUserGoal.GoalTempelate.targetMuscleMass = newUserGoal.GoalTempelate.targetMuscleMass;
//                }


//                // Save changes to the database


//                await _userGoalRepo.AddAsync(newUserGoal);
//                return new OkObjectResult("Goal successfully updated to a new goal.");
//            }

//        }
//    }
//}
