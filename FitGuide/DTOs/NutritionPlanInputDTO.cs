namespace FitGuide.DTOs
{
    public class NutritionPlanInputDTO
    {
        public string? Name { get; set; }
        public string UserId { get; set; }
        public double CaloriestTarget { get; set; }
        public double ProteinTarget { get; set; }
        public double CarbsTarget { get; set; }
        public double FatTarget { get; set; }
        public double GymFrequency { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
