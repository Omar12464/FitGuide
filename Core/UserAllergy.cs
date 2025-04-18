using Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Identity.Entities
{
    public class UserAllergy : ModelBase
    {
        public string UserId { get; set; }
        public List<int> AllergyId { get; set; } = new List<int>();

    }
}
