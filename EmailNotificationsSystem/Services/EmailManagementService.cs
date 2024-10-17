using EmailNotificationsSystem.Models;
using EmailNotificationsSystem.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace EmailNotificationsSystem.Services
{
    public class EmailManagementService : IEmailManagementService
    {
        private readonly IDistributedCache _redisCache;

        public EmailManagementService(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        public async Task<List<EmailModel>> GetEmailsAsync()
        {
            List<EmailModel> emails = new List<EmailModel>();

            string cachedEmails = await _redisCache.GetStringAsync("emails");

            if (!string.IsNullOrEmpty(cachedEmails))
            {
                List<string> emailKeys = JsonConvert.DeserializeObject<List<string>>(cachedEmails);

                foreach (string emailKey in emailKeys)
                {
                    string emailJson = await _redisCache.GetStringAsync(emailKey);
                    EmailModel email = JsonConvert.DeserializeObject<EmailModel>(emailJson);

                    if (email != null)
                        emails.Add(email);
                }
                return emails;
            }

            return new List<EmailModel>();
        }

        public async Task SaveEmailAsync(EmailModel email)
        {
            string emailJson = JsonConvert.SerializeObject(email);
            string emailKey = $"email_{email.ClientId}_{email.TemplateId}";
            await _redisCache.SetStringAsync(emailKey, emailJson);
            await SaveEmailKeyAsync(emailKey);
        }

        private async Task SaveEmailKeyAsync(string emailKey)
        {
            List<string> emailKeys = new List<string>();
            string emails = await _redisCache.GetStringAsync("emails");

            if (string.IsNullOrEmpty(emails))
            {
                emails = emailKey;
                emailKeys.Add(emailKey);
            }
            else
            {
                List<string> existingEmails = JsonConvert.DeserializeObject<List<string>>(emails);

                if (existingEmails != null)
                {
                    existingEmails.Add(emailKey);
                    emails = JsonConvert.SerializeObject(existingEmails);
                    await _redisCache.SetStringAsync("emails", emails);
                    return;
                }
            }

            await _redisCache.SetStringAsync("emails", JsonConvert.SerializeObject(emailKeys));
        }
    }
}
