using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Identity.Entities
{
    public class UserInjury
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<int> InjuryId { get; set; }= new List<int>();

    }
}
