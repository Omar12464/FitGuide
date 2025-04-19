using System.ComponentModel.DataAnnotations;

namespace FitGuide.DTOs
{
    public class ResetPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Required]
        public string Token { get; set; }
    }
} 