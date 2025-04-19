using Core.Identity.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Core.Identity.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _smtpServer = _configuration["EmailSettings:SmtpServer"] ?? throw new ArgumentNullException(nameof(_smtpServer));
            _smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
            _smtpUsername = _configuration["EmailSettings:SmtpUsername"] ?? throw new ArgumentNullException(nameof(_smtpUsername));
            _smtpPassword = _configuration["EmailSettings:SmtpPassword"] ?? throw new ArgumentNullException(nameof(_smtpPassword));
            _fromEmail = _configuration["EmailSettings:FromEmail"] ?? throw new ArgumentNullException(nameof(_fromEmail));
            _fromName = _configuration["EmailSettings:FromName"] ?? "FitGuide";
        }

        public async Task SendEmailConfirmationAsync(string email, string confirmationLink)
        {
            var subject = "Confirm your email";
            var body = $@"
                <h2>Welcome to FitGuide!</h2>
                <p>Please confirm your email by clicking the link below:</p>
                <p><a href='{confirmationLink}'>Confirm Email</a></p>
                <p>If you didn't create an account with us, please ignore this email.</p>";

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendPasswordResetEmailAsync(string email, string resetLink)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException(nameof(email));

            var subject = "Reset Your Password";
            var body = $@"
                <h2>Reset Your Password</h2>
                <p>You have requested to reset your password. Click the link below to proceed:</p>
                <p><a href='{resetLink}'>Reset Password</a></p>
                <p>If you didn't request a password reset, please ignore this email.</p>";

            await SendEmailAsync(email, subject, body);
        }

        // public async Task SendWelcomeEmailAsync(string email, string userName)
        // {
        //     var subject = "Welcome to FitGuide!";
        //     var body = $@"
        //         <h2>Welcome {userName}!</h2>
        //         <p>Thank you for joining FitGuide. We're excited to have you on board!</p>
        //         <p>Start your fitness journey today by exploring our features:</p>
        //         <ul>
        //             <li>Create personalized workout plans</li>
        //             <li>Track your progress</li>
        //             <li>Set and achieve your fitness goals</li>
        //         </ul>
        //         <p>If you have any questions, feel free to contact our support team.</p>";

        //     await SendEmailAsync(email, subject, body);
        // }

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            using var client = new SmtpClient(_smtpServer, _smtpPort)
            {
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                EnableSsl = true
            };

            var message = new MailMessage
            {
                From = new MailAddress(_fromEmail, _fromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };
            message.To.Add(to);

            await client.SendMailAsync(message);
        }
    }
} 