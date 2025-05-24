namespace FitGuide.DTOs
{
    public class ExerciseDetailsDTO
    {
        public string ExerciseName { get; set; }
        public int Reps { get; set; }
        public int Sets { get; set; }
        public double? MaxWeight { get; set; }
    }
}
