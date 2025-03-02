using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Identity.Entities
{
    public class UserAllergy
    {
        public string UserId { get; set; }
        public int AllergyId { get; set; }
        public User user { get; set; }
        public Allergy allergy { get; set; }
    }
}
