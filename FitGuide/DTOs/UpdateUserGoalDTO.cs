namespace FitGuide.DTOs
{
    public class UpdateUserGoalDTO
    {
        public string  name { get; set; }
        public float TargetWeight { get; set; }
        public float? TargetMuscleMass { get; set; }
        public float? TargetBMI { get; set; }
    }
}
