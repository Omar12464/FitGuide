using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Identity.Entities
{
    public class User:IdentityUser
    {

        public int AllergiesId { get; set; }
        public int InjuriesId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }= DateTimeOffset.UtcNow;

        //public ICollection<Allergy> allergies { get; set; }
        //public ICollection<Injury> injuries { get; set; }
        public ICollection<UserAllergy> userAllergies { get; set; } = new HashSet<UserAllergy>();
        public ICollection<UserInjury> userInjuries { get; set; } = new HashSet<UserInjury>();
        public ICollection<WorkOutPlan> workOutPlans { get; set; }=new HashSet<WorkOutPlan>();
        public ICollection<UserMetrics> userMetrics { get; set; } = new HashSet<UserMetrics>();
        public ICollection<UserGoal> userGoals { get; set; } = new HashSet<UserGoal>();



    }
}
