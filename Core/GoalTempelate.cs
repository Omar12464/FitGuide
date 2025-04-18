using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class GoalTempelate:ModelBase
    {
        public string? name { get; set; }
        public float? targetBMI { get; set; }
        public float? targetWeight { get; set; }
        public float? targetMuscleMass { get; set; }
        public float? targetWaterMass { get; set; }
        public string? description { get; set; }
        public string? ageGroup { get; set; }
    }
}
