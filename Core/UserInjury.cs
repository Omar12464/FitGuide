using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Identity.Entities
{
    public class UserInjury : ModelBase
    {
        public string UserId { get; set; }
        public int injuryId { get; set; } 
        public Injury injury { get; set; }

    }
}
