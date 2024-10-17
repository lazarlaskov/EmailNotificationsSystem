using EmailNotificationsSystem.Models;

namespace EmailNotificationsSystem.Services.Interfaces
{
    public interface IEmailTemplateService
    {
        Task<IEnumerable<EmailTemplateModel>> GetEmailTemplatesAsync();
        Task UpdateEmailTemplateAsync(EmailTemplateModel emailTemplate);
    }
}
