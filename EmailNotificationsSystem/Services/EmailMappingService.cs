using EmailNotificationsSystem.Models;
using EmailNotificationsSystem.Services.Interfaces;
using System.Xml.Linq;

namespace EmailNotificationsSystem.Services
{
    public class EmailMappingService : IEmailMappingService
    {
        private readonly ILogger<EmailMappingService> _logger;

        public EmailMappingService(ILogger<EmailMappingService> logger)
        {
            _logger = logger;
        }

        public async Task<EmailModel> CreateEmailContentAsync(ClientModel client, ClientDataModel clientData)
        {
            XDocument email = new(
                new XElement("Clients",
                    new XElement("Client",
                        new XAttribute("ID", client.ClientId),
                    new XElement("Template",
                        new XAttribute("ID", clientData.TemplateId),
                        new XElement("Name", clientData.TemplateName),
                        new XElement("MarketingData", clientData.MarketingData)
                        )
                    )
                )
            );

            return await MapEmailXmlToModelAsync(email);
        }

        public async Task<EmailModel> MapEmailDataFromStringAsync(string emailContent)
        {
            XDocument emailXml = await MapEmailContentToXmlAsync(emailContent);

            if (emailXml == null)
            {
                return null;
            }

            return await MapEmailXmlToModelAsync(emailXml);
        }

        public async Task<List<EmailModel>> MapBulkEmailDataFromStringAsync(string emailContent)
        {
            XDocument emailsXml = await MapEmailContentToXmlAsync(emailContent);

            if (emailsXml == null)
            {
                return null;
            }

            List<XElement> emailClients = emailsXml
                .Descendants("Clients").Select(el => el).ToList();

            return await MapEmailClientsToModelsAsync(emailClients);
        }

        private async Task<EmailModel> MapEmailXmlToModelAsync(XDocument emailXml)
        {
            XElement clientIdElement = emailXml.Root.Element("Client");
            XElement templateElement = clientIdElement.Element("Template");
            XElement marketingDataElement = templateElement.Element("MarketingData");

            if (clientIdElement == null || templateElement == null || marketingDataElement == null)
            {
                return null;
            }

            EmailModel email = new EmailModel
            {
                ClientId = Int32.Parse(clientIdElement.FirstAttribute.Value),
                TemplateId = Int32.Parse(templateElement.FirstAttribute.Value),
                MarketingData = marketingDataElement.Value
            };

            return email;
        }

        private async Task<List<EmailModel>> MapEmailClientsToModelsAsync(List<XElement> emailClients)
        {
            List<EmailModel> emailModels = new List<EmailModel>();

            foreach (XElement emailClient in emailClients)
            {
                XElement clientIdElement = emailClient.Element("Client");
                XElement templateElement = clientIdElement.Element("Template");
                XElement marketingDataElement = templateElement.Element("MarketingData");

                if (clientIdElement == null || templateElement == null || marketingDataElement == null)
                {
                    return null;
                }

                EmailModel email = new EmailModel
                {
                    ClientId = Int32.Parse(clientIdElement.FirstAttribute.Value),
                    TemplateId = Int32.Parse(templateElement.FirstAttribute.Value),
                    MarketingData = marketingDataElement.Value
                };

                emailModels.Add(email);
            }

            return emailModels;
        }

        private Task<XDocument> MapEmailContentToXmlAsync(string emailContent)
        {
            try
            {
                XDocument emailXml = XDocument.Parse(emailContent);
                return Task.FromResult(emailXml);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing email content to XML.");
            }

            return Task.FromResult<XDocument>(null);
        }
    }
}
