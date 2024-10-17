using EmailNotificationsSystem.Models;

namespace EmailNotificationsSystem.Services.Interfaces
{
    public interface IEmailAdministrationService
    {
        Task<EmailConfigurationViewModel> GetEmailConfigurationAsync();
        Task UpdateEmailConfigurationAsync(EmailConfigurationModel emailConfiguration);
    }
}
