using EmailNotificationsSystem.Models;
using EmailNotificationsSystem.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace EmailNotificationsSystem.Services
{
    public class ClientPreparationService : IClientPreparationService
    {
        private readonly IDistributedCache _redisCache;

        public ClientPreparationService(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        public async Task<IEnumerable<ClientModel>> PrepareClientsAsync()
        {
            IEnumerable<ClientModel> clients = new List<ClientModel>
            {
                new ClientModel
                {
                    ClientId = 12345,
                    ClientName = "Client 1",
                },
                new ClientModel
                {
                    ClientId = 54321,
                    ClientName = "Client 2",
                }
            };

            string clientsJson = JsonConvert.SerializeObject(clients);
            await _redisCache.SetStringAsync("clients", clientsJson);

            return clients;
        }

        public async Task<List<ClientModel>> PrepareClientsForMassSendingAsync()
        {
            List<ClientModel> clients = new List<ClientModel>();

            for (int i = 0; i < 100000; i++)
            {
                clients.Add(new ClientModel
                {
                    ClientId = i,
                    ClientName = $"Client {i}",
                });
            }

            string clientsJson = JsonConvert.SerializeObject(clients);
            await _redisCache.SetStringAsync("clients", clientsJson);

            return clients;
        }

        public async Task PrepareClientsDataForMassSendingAsync()
        {
            string cachedClients = await _redisCache.GetStringAsync("clients");
            List<ClientModel> clients = JsonConvert.DeserializeObject<List<ClientModel>>(cachedClients);

            foreach (ClientModel client in clients)
            {
                ClientDataModel clientData = new ClientDataModel
                {
                    ClientId = client.ClientId,
                    TemplateId = client.ClientId,
                    TemplateName = $"TemplateName{client.ClientId}.html",
                    MarketingData = "{\"Content\": \"Email Content\"}"
                };

                await _redisCache.SetStringAsync($"clientData_{client.ClientId}",
                    JsonConvert.SerializeObject(clientData));
            }
        }

        public async Task PrepareClientsDataAsync()
        {
            ClientDataModel firstClientData = new ClientDataModel
            {
                ClientId = 12345,
                TemplateId = 1,
                TemplateName = "TemplateName.html",
                MarketingData = "{\"Content\": \"Email Content\"}"
            };

            if (await _redisCache.GetStringAsync($"clientData_{firstClientData.ClientId}") == null)
            {
                await _redisCache.SetStringAsync($"clientData_{firstClientData.ClientId}",
                    JsonConvert.SerializeObject(firstClientData));
            }

            ClientDataModel secondClientData = new ClientDataModel
            {
                ClientId = 54321,
                TemplateId = 2,
                TemplateName = "TemplateName2.html",
                MarketingData = "{\"Content\": \"Email Content\"}"
            };

            if (await _redisCache.GetStringAsync($"clientData_{secondClientData.ClientId}") == null)
            {
                await _redisCache.SetStringAsync($"clientData_{secondClientData.ClientId}",
                    JsonConvert.SerializeObject(secondClientData));
            }
        }

        public async Task<IEnumerable<ClientModel>> GetClientsAsync(bool isForMassSending)
        {
            string cachedClients = await _redisCache.GetStringAsync("clients");

            if (cachedClients != null)
            {
                IEnumerable<ClientModel> clients = JsonConvert
                    .DeserializeObject<IEnumerable<ClientModel>>(cachedClients);

                if (clients != null && !isForMassSending)
                {
                    return clients;
                }

                if (isForMassSending)
                {
                    await PrepareClientsForMassSendingAsync();
                    await PrepareClientsDataForMassSendingAsync();
                }
            }
            else
            {
                if (!isForMassSending)
                {
                    await PrepareClientsAsync();
                    await PrepareClientsDataAsync();
                }
                else
                {
                    await PrepareClientsForMassSendingAsync();
                    await PrepareClientsDataForMassSendingAsync();
                }
            }

            cachedClients = await _redisCache.GetStringAsync("clients");

            return JsonConvert
                    .DeserializeObject<IEnumerable<ClientModel>>(cachedClients);
        }

        public async Task<ClientModel> GetClientDataAsync(int clientId)
        {
            IEnumerable<ClientModel> clients = await GetClientsAsync(false);

            if (clients != null)
            {
                return clients.FirstOrDefault(c => c.ClientId == clientId);
            }

            return null;
        }
    }
}
