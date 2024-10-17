using EmailNotificationsSystem.Models;
using EmailNotificationsSystem.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace EmailNotificationsSystem.Services
{
    public class ClientManagementService : IClientManagementService
    {
        private readonly IDistributedCache _redisCache;

        public ClientManagementService(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        public async Task<ClientDataModel> GetClientDataAsync(int clientId)
        {
            string redisClientData = await _redisCache.GetStringAsync($"clientData_{clientId}");

            if (!string.IsNullOrEmpty(redisClientData))
            {
                return JsonConvert.DeserializeObject<ClientDataModel>(redisClientData);
            }

            return new ClientDataModel();
        }

        public async Task UpdateClientDataAsync(ClientDataModel clientData)
        {
            // Update client data
            await _redisCache.SetStringAsync($"clientData_{clientData.ClientId}", JsonConvert.SerializeObject(clientData));
        }
    }
}
