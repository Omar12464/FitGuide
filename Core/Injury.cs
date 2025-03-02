using Core.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Injury
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserInjury> user { get; set; }=new HashSet<UserInjury>();
        //public ICollection<UserAllergy> userAllergies { get; set; } = new HashSet<UserAllergy>();

    }
}
