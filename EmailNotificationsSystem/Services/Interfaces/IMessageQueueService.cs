using EmailNotificationsSystem.Models;

namespace EmailNotificationsSystem.Services.Interfaces
{
    public interface IMessageQueueService
    {
        void InitializeMessageQueue(string queuePath);
        Task SendEmailMessagesAsync(IEnumerable<EmailModel> emailMessages);
    }
}
