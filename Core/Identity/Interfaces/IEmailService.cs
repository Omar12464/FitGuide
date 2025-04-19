namespace Core.Identity.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailConfirmationAsync(string email, string confirmationLink);
        Task SendPasswordResetEmailAsync(string email, string resetLink);
        //Task SendWelcomeEmailAsync(string email, string userName);
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = true);
    }
} 