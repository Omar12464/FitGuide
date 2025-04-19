using System.ComponentModel.DataAnnotations;

namespace FitGuide.DTOs
{
    public class ForgotPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
} 