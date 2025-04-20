using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class WorkOutExercises:ModelBase
    {
        public int WorkoOutId { get; set; }
        public WorkOutPlan workOutPlan { get; set; }
        public int ExerciseId { get; set; }
        public Exercise exercise { get; set; }
        public int NumberOfSets { get; set; }
        public int NumberOfReps { get; set; } = 10;
    }
}
