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
        public string FistName { get; set; }
        public string LastName { get; set; }
        
        public string FullName =>$"{FistName} {LastName}".Trim();
        public string Gender { get; set; }
        public string Country { get; set; }

        public int Age { get; set; }

        public DateTimeOffset CreatedAt { get; set; }= DateTimeOffset.UtcNow;

        //public ICollection<UserAllergy> userallergies { get; set; } = new HashSet<UserAllergy>();
        //public ICollection<UserInjury> userinjuries { get; set; } = new HashSet<UserInjury>();
        //public ICollection<WorkOutPlan> workoutplans { get; set; } = new HashSet<WorkOutPlan>();
        //public ICollection<UserMetrics> usermetrics { get; set; } = new HashSet<UserMetrics>();
        //public ICollection<UserGoal> usergoals { get; set; } = new HashSet<UserGoal>();



    }
}
