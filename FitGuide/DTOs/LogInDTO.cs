using System.ComponentModel.DataAnnotations;

namespace FitGuide.DTOs
{
    public class LogInDTO
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
