using EmailNotificationsSystem.Models;

namespace EmailNotificationsSystem.Services.Interfaces
{
    public interface IEmailManagementService
    {
        Task SaveEmailAsync(EmailModel email);
        Task<List<EmailModel>> GetEmailsAsync();
    }
}
