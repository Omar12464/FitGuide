using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class ExerciseLog:ModelBase
    {
        public string UserId { get; set; }
        public byte[] VideoFeedback { get; set; }
        public DateTime LoggedAt { get; set; }
        public int WorkOutExerciseId { get; set; }
        public WorkOutExercises workOutExercises { get; set; }
        public int Exercise_FeedbackId { get; set; }
        public ICollection<Exercise_Feedback> exercise_Feedback { get; set; } = new HashSet<Exercise_Feedback>();

    }
}
