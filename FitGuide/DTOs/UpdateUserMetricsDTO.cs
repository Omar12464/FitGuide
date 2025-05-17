using Core;
using System.ComponentModel.DataAnnotations;

namespace FitGuide.DTOs
{
    public class UpdateUserMetricsDTO
    {
        [Range(50, 300, ErrorMessage = "Height must be between 50 and 300 kg.")]
        public float? Weight { get; set; }
        [Range(1, 300, ErrorMessage = "Height must be between 1 and 300 cm.")]
        public float? Height { get; set; }
        public float? Fat { get; set; }
        public float? MuscleMass { get; set; }
        public float? WaterMass { get; set; }
        public FitnessLevel? fitnessLevel { get; set; }
        public double GymFrequency { get; set; }

    }
}
