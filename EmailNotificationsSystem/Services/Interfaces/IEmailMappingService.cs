using EmailNotificationsSystem.Models;
using System.Xml.Linq;

namespace EmailNotificationsSystem.Services.Interfaces
{
    public interface IEmailMappingService
    {
        Task<EmailModel> CreateEmailContentAsync(ClientModel client, ClientDataModel clientData);
        Task<EmailModel> MapEmailDataFromStringAsync(string emailContent);
        Task<List<EmailModel>> MapBulkEmailDataFromStringAsync(string emailContent);
    }
}
