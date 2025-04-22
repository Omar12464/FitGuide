using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Services
{
    public interface IGenrateWorkOutService
    {
        public Task<List<WorkOutPlan>> GetWorkOut(string userId);
        public Task<List<Exercise>> FilterExrcises(string userId);
        public  Task GeneratePersonalizedPlans(string userId, string PlanType);
        public List<Exercise> GetSafeExercisesForCategory(IEnumerable<Exercise> exercises, IEnumerable<string> muscleGroups, IEnumerable<string> injuryAffectedParts);
        public Task<WorkOutExercises> GetWorkOutsPlansForUser(string userId);




    }
}
