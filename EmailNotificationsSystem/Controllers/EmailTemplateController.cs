using EmailNotificationsSystem.Models;
using EmailNotificationsSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmailNotificationsSystem.Controllers
{
    public class EmailTemplateController : Controller
    {
        private readonly IEmailTemplateService _emailTemplateService;

        public EmailTemplateController(IEmailTemplateService emailTemplateService)
        {
            _emailTemplateService = emailTemplateService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<EmailTemplateModel> emailTemplates = await _emailTemplateService.GetEmailTemplatesAsync();
            return View(emailTemplates);
        }

        [HttpGet]
        public async Task<IActionResult> EditTemplate(int templateId)
        {
            IEnumerable<EmailTemplateModel> emailTemplates = 
                await _emailTemplateService.GetEmailTemplatesAsync();

            EmailTemplateModel emailTemplate = emailTemplates
                .Where(et => et.TemplateId == templateId).First();

            return View(emailTemplate);
        }

        [HttpPost]
        public async Task<IActionResult> EditTemplate(EmailTemplateModel emailTemplate)
        {
            await _emailTemplateService.UpdateEmailTemplateAsync(emailTemplate);
            return RedirectToAction("Index");
        }
    }
}
