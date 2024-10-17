using EmailNotificationsSystem.Models;

namespace EmailNotificationsSystem.Services.Interfaces
{
    public interface IEmailHelperService
    {
        Task<List<EmailModel>> CreateEmailFromClientDataAsync(string emailContent);
        Task<List<EmailModel>> CreateMultipleEmailsFromClientDataAsync(string emailContent);
        Task<List<EmailModel>> CreateMassEmailsFromClientDataAsync(string emailContent);

    }
}
