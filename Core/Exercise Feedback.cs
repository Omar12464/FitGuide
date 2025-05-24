    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace Core
    {
        public class Exercise_Feedback:ModelBase
        {
            public string ExerciseFeedback { get; set; }
            public int ExerciseLogId { get; set; }
            public ExerciseLog  exerciseLog { get; set; }

    }
    }
