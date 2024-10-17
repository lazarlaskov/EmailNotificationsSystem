using EmailNotificationsSystem.Models;
using EmailNotificationsSystem.Services.Interfaces;

namespace EmailNotificationsSystem.Services
{
    public class EmailHelperService : IEmailHelperService
    {
        private readonly ILogger<EmailHelperService> _logger;
        private readonly IClientPreparationService _clientPreparationService;
        private readonly IClientManagementService _clientManagementService;
        private readonly IEmailMappingService _emailMappingService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IEmailAdministrationService _emailAdministrationService;

        public EmailHelperService(ILogger<EmailHelperService> logger, 
            IClientPreparationService clientPreparationService, 
            IClientManagementService clientManagementService,
            IEmailMappingService emailMappingService,
            IEmailTemplateService emailTemplateService,
            IEmailAdministrationService emailAdministrationService)
        {
            _logger = logger;
            _clientPreparationService = clientPreparationService;
            _clientManagementService = clientManagementService;
            _emailMappingService = emailMappingService;
            _emailTemplateService = emailTemplateService;
            _emailAdministrationService = emailAdministrationService;
        }

        public async Task<List<EmailModel>> CreateEmailFromClientDataAsync(string emailContent)
        {
            EmailModel email = null;

            EmailConfigurationModel emailConfiguration = await _emailAdministrationService.GetEmailConfigurationAsync();

            if (emailConfiguration == null)
            {
                _logger.LogError("Email configuration not found.");
                return null;
            }

            if (string.IsNullOrEmpty(emailContent))
            {
                IEnumerable<ClientModel> clients = await _clientPreparationService.GetClientsAsync(false);

                if (clients.Any())
                {
                    ClientModel client = clients.First();
                    ClientDataModel clientData = await _clientManagementService.GetClientDataAsync(client.ClientId);
                    
                    IEnumerable<EmailTemplateModel> emailTemplates = await _emailTemplateService.GetEmailTemplatesAsync();
                    if (!emailTemplates.Any(et => et.TemplateId == clientData.TemplateId))
                    {
                        _logger.LogError($"Template with ID {clientData.TemplateId} not found.");
                        return null;
                    }

                    email = await _emailMappingService.CreateEmailContentAsync(client, clientData);
                }
            }
            else
            {
                email = await _emailMappingService.MapEmailDataFromStringAsync(emailContent);
            }

            return new List<EmailModel> { email };
        }

        public async Task<List<EmailModel>> CreateMultipleEmailsFromClientDataAsync(string emailContent)
        {
            List<EmailModel> emails = new List<EmailModel>();

            EmailConfigurationModel emailConfiguration = await _emailAdministrationService.GetEmailConfigurationAsync();

            if (emailConfiguration == null)
            {
                _logger.LogError("Email configuration not found.");
                return null;
            }

            if (string.IsNullOrEmpty(emailContent))
            {
                IEnumerable<ClientModel> clients = await _clientPreparationService.GetClientsAsync(false);

                IEnumerable<EmailTemplateModel> emailTemplates = await _emailTemplateService.GetEmailTemplatesAsync();

                if (clients.Any())
                {
                    foreach (ClientModel client in clients)
                    {
                        ClientDataModel clientData = await _clientManagementService.GetClientDataAsync(client.ClientId);

                        if (!emailTemplates.Any(et => et.TemplateId == clientData.TemplateId))
                        {
                            _logger.LogError($"Template with ID {clientData.TemplateId} not found.");
                            return null;
                        }

                        EmailModel email = await _emailMappingService.CreateEmailContentAsync(client, clientData);
                        emails.Add(email);
                    }
                }
            }
            else
            {
                emails = await _emailMappingService
                    .MapBulkEmailDataFromStringAsync(emailContent);
            }

            return emails;
        }

        public async Task<List<EmailModel>> CreateMassEmailsFromClientDataAsync(string emailContent)
        {
            List<EmailModel> emails = new List<EmailModel>();

            EmailConfigurationModel emailConfiguration = await _emailAdministrationService.GetEmailConfigurationAsync();

            if (emailConfiguration == null)
            {
                _logger.LogError("Email configuration not found.");
                return null;
            }

            if (string.IsNullOrEmpty(emailContent))
            {
                IEnumerable<ClientModel> clients = await _clientPreparationService.GetClientsAsync(true);

                if (clients.Any())
                {
                    foreach (ClientModel client in clients)
                    {
                        ClientDataModel clientData = await _clientManagementService.GetClientDataAsync(client.ClientId);

                        // We don't check for email templates here because we are sending mass pre-defined emails

                        EmailModel email = await _emailMappingService.CreateEmailContentAsync(client, clientData);
                        emails.Add(email);
                    }
                }
            }
            else
            {
                emails = await _emailMappingService
                    .MapBulkEmailDataFromStringAsync(emailContent);
            }

            return emails;
        }
    }
}
