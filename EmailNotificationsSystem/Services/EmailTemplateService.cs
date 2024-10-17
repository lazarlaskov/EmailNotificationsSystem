using EmailNotificationsSystem.Models;
using EmailNotificationsSystem.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace EmailNotificationsSystem.Services
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly IDistributedCache _redisCache;

        public EmailTemplateService(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        public async Task<IEnumerable<EmailTemplateModel>> GetEmailTemplatesAsync()
        {
            string cachedTemplates = await _redisCache.GetStringAsync("emailTemplates");

            if (!string.IsNullOrEmpty(cachedTemplates))
            {
                return JsonConvert
                    .DeserializeObject<IEnumerable<EmailTemplateModel>>(cachedTemplates);
            }

            await PrepareEmailTemplatesAsync();

            return JsonConvert.DeserializeObject<IEnumerable<EmailTemplateModel>>(
                await _redisCache.GetStringAsync("emailTemplates")
            );
        }

        public async Task UpdateEmailTemplateAsync(EmailTemplateModel emailTemplate)
        {
            IEnumerable<EmailTemplateModel> emailTemplates = 
                await GetEmailTemplatesAsync();

            EmailTemplateModel templateToUpdate = emailTemplates
                .Where(et => et.TemplateId == emailTemplate.TemplateId).First();

            templateToUpdate.TemplateName = emailTemplate.TemplateName;
            templateToUpdate.TemplateContent = emailTemplate.TemplateContent;

            string emailTemplatesJson = JsonConvert.SerializeObject(emailTemplates);
            await _redisCache.SetStringAsync("emailTemplates", emailTemplatesJson);
        }

        private async Task PrepareEmailTemplatesAsync()
        {
            EmailTemplateModel firstTemplate = new EmailTemplateModel
            {
                TemplateId = 1,
                TemplateName = "TemplateName.html",
                TemplateContent = "This is the first template body"
            };

            EmailTemplateModel secondTemplate = new EmailTemplateModel
            {
                TemplateId = 2,
                TemplateName = "TemplateName2.html",
                TemplateContent = "This is the second template body"
            };

            List<EmailTemplateModel> emailTemplates = new List<EmailTemplateModel>
            {
                firstTemplate,
                secondTemplate
            };

            string emailTemplatesJson = JsonConvert.SerializeObject(emailTemplates);
            await _redisCache.SetStringAsync("emailTemplates", emailTemplatesJson);
        }
    }
}