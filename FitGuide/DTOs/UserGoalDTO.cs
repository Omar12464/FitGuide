namespace FitGuide.DTOs
{
    public class UserGoalDTO
    {
        public string? name { get; set; }
        public float? targetBMI { get; set; }
        public float? targetWeight { get; set; }
        public float? targetMuscleMass { get; set; }
        public float? targetWaterMass { get; set; }
        public string description { get; set; }
    }
}
