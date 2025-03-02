using Core.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Allergy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserAllergy> users { get; set; }=new HashSet<UserAllergy>();
    }
}
