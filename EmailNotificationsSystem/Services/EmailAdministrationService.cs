using EmailNotificationsSystem.Models;
using EmailNotificationsSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace EmailNotificationsSystem.Services
{
    public class EmailAdministrationService : IEmailAdministrationService
    {
        private readonly IDistributedCache _redisCache;

        public EmailAdministrationService(IDistributedCache distributedCache)
        {
            _redisCache = distributedCache;
        }

        public async Task<EmailConfigurationViewModel> GetEmailConfigurationAsync()
        {
            string cachedEmailConfiguration = 
                await _redisCache.GetStringAsync("emailConfiguration");

            if (!string.IsNullOrEmpty(cachedEmailConfiguration))
            {
                EmailConfigurationViewModel model = 
                    JsonConvert.DeserializeObject<EmailConfigurationViewModel>(
                        cachedEmailConfiguration
                    );

                model.Protocols = GetProtocols();

                model.SecurityModes = GetSecurityModes();

                return model;
            }

            EmailConfigurationViewModel emailConfiguration = 
                new EmailConfigurationViewModel
                {
                    ProtocolId = 1,
                    Port = 25,
                    DefaultSender = "info@lazarmedia.com",
                    HostName = "smtp.lazarmedia.com",
                    SecurityModeId = 1,
                    Protocols = GetProtocols(),
                    SecurityModes = GetSecurityModes()
                };

            await _redisCache
                .SetStringAsync("emailConfiguration", 
                JsonConvert.SerializeObject(emailConfiguration));

            return emailConfiguration;
        }

        public async Task UpdateEmailConfigurationAsync(EmailConfigurationModel emailConfiguration)
        {
            await _redisCache.SetStringAsync("emailConfiguration", 
                JsonConvert.SerializeObject(emailConfiguration));
        }

        private IEnumerable<SelectListItem> GetProtocols()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "SMTP" },
                new SelectListItem { Value = "2", Text = "SMTPS" },
            };
        }

        private IEnumerable<SelectListItem> GetSecurityModes()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "SSL" },
                new SelectListItem { Value = "2", Text = "TLS" },
            };
        }
    }
}
