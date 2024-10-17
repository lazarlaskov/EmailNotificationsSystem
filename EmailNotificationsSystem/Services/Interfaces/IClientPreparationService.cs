using EmailNotificationsSystem.Models;

namespace EmailNotificationsSystem.Services.Interfaces
{
    public interface IClientPreparationService
    {
        Task<IEnumerable<ClientModel>> PrepareClientsAsync();
        Task PrepareClientsDataAsync();
        Task<IEnumerable<ClientModel>> GetClientsAsync(bool isForMassSending);
    }
}
