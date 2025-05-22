using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core
{
    public class WorkOutExercises:ModelBase
    {
        public string WorkOutName { get; set; }
        public int WorkoOutId { get; set; }
        [JsonIgnore]
        public WorkOutPlan workOutPlan { get; set; }
        public int ExerciseId { get; set; }
        [JsonIgnore]
        public Exercise exercise { get; set; }
        public int NumberOfSets { get; set; } = 3;
        public int NumberOfReps { get; set; } = 10;
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; } = false;
        public double? Weight { get; set; }
    }
}
