namespace FitGuide.DTOs
{
    public class WorkOutExercisesResponseDTO
    {
        public string UserId { get; set; }
        public int ExerciseId { get; set; }
        public ExerciseDto Exercise { get; set; } // Nested DTO for Exercise details
        public WorkOutPlanDto WorkOutPlan { get; set; } // Nested DTO for WorkOutPlan details
        public int WorkoOutId { get; set; }
        public string WorkOutName { get; set; }
        public int NumberOfReps { get; set; }
        public int NumberOfSets { get; set; }
    }

    public class ExerciseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Difficulty { get; set; }
        public string? TypeOfMachine { get; set; }
        public string? TargetMuscle { get; set; }
        public List<string>? TargetInjury { get; set; }
    }

    public class WorkOutPlanDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfDays { get; set; }
        public string DifficultyLevel { get; set; }
    }
}