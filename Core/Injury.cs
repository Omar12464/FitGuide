using Core;
using Core.Identity.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Injury : ModelBase
    {
        public string Name { get; set; }
        public string AffectedBodyPart { get; set; }
        public List<string>? ContraindicatedExercises { get; set; }
        public List<string>? SuitableExercises { get; set; }
        public List<string>? SuitableEquipment { get; set; }
        public ICollection<UserInjury> UserInjuries { get; set; } =new HashSet<UserInjury>();


    }
}
