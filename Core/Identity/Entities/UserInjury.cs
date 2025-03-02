using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Identity.Entities
{
    public class UserInjury
    {
        public string UserId { get; set; }
        public int InjuryId { get; set; }
        public User user { get; set; }
        public Injury injury { get; set; }
    }
}
