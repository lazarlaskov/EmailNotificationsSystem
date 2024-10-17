using EmailNotificationsSystem.Models;
using EmailNotificationsSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmailNotificationsSystem.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailManagementService _emailManagementService;
        private readonly IEmailHelperService _emailHelperService;
        private readonly IMessageQueueService _messageQueueService;

        public EmailController(
            IEmailManagementService emailManagementService,
            IEmailHelperService emailHelperService,
            IMessageQueueService messageQueueService)
        {
            _emailManagementService = emailManagementService;
            _emailHelperService = emailHelperService;
            _messageQueueService = messageQueueService;
        }

        public async Task<IActionResult> Index()
        {
            List<EmailModel> emails = await _emailManagementService.GetEmailsAsync();
            _messageQueueService.InitializeMessageQueue("emails");
            return View(emails);
        }

        [HttpPost]
        public async Task<IActionResult> Send(string emailContent)
        {
            List<EmailModel> emails = await _emailHelperService
                .CreateEmailFromClientDataAsync(emailContent);

            if (!emails.Any())
            {
                return View("Index", emails);
            }
            else
            {
                await _messageQueueService.SendEmailMessagesAsync(
                    emails
                );
            }

            List<EmailModel> processedEmails = await _emailManagementService.GetEmailsAsync();

            return View("Index", processedEmails);
        }

        [HttpPost]
        public async Task<IActionResult> SendMultiple(string emailContent)
        {
            List<EmailModel> emails = await _emailHelperService
                .CreateMultipleEmailsFromClientDataAsync(emailContent);

            if (!emails.Any())
            {
                return View("Index", emails);
            }
            else
            {
                await _messageQueueService.SendEmailMessagesAsync(emails);
            }

            List<EmailModel> processedEmails = await _emailManagementService.GetEmailsAsync();

            return View("Index", processedEmails);
        }

        [HttpPost]
        public async Task<IActionResult> MassSend(string emailContent)
        {
            List<EmailModel> emails = await _emailHelperService
                .CreateMassEmailsFromClientDataAsync(emailContent);

            if (!emails.Any())
            {
                return View("Index", emails);
            }
            else
            {
                await _messageQueueService.SendEmailMessagesAsync(emails);
            }

            List<EmailModel> processedEmails = await _emailManagementService.GetEmailsAsync();

            return View("Index", processedEmails);
        }
    }
}
