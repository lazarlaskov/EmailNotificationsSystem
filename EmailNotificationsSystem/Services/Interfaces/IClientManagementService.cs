using EmailNotificationsSystem.Models;

namespace EmailNotificationsSystem.Services.Interfaces
{
    public interface IClientManagementService
    {
        Task<ClientDataModel> GetClientDataAsync(int clientId);
        Task UpdateClientDataAsync(ClientDataModel clientData);
    }
}
