using Core.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class UserData
    {
        public UserMetrics userMetrics { get; set; } = new UserMetrics();
        public UserGoal userGoal { get; set; }=new UserGoal();
        public List<UserInjury> userInjury { get; set; }= new List<UserInjury>();
    }
}
