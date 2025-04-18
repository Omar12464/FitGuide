using System.ComponentModel.DataAnnotations;

namespace FitGuide.DTOs
{
    public class UserMetricsDTO
    {
        [Required(AllowEmptyStrings =false,ErrorMessage ="Must Enter Weight")]
        [Range(50, 300, ErrorMessage = "Height must be between 50 and 300 kg.")]
        public float Weight { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Must Enter Height")]
        [Range(1, 300, ErrorMessage = "Height must be between 1 and 300 cm.")]
        public float Height { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Must Enter Height")]
        public float Fat { get; set; }
        public float? MuscleMass { get; set; }
        public float? WaterMass { get; set; }
    }
}
