namespace FitGuide.DTOs
{
    public class UserDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public DateTimeOffset CreatedAt { get; set; } 
    }
}
