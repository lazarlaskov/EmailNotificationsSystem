using EmailNotificationsSystem.Models;
using EmailNotificationsSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmailNotificationsSystem.Controllers
{
    public class EmailAdministrationController : Controller
    {
        private readonly IEmailAdministrationService _emailAdministrationService;

        public EmailAdministrationController(
            IEmailAdministrationService emailAdministrationService)
        {
            _emailAdministrationService = emailAdministrationService;
        }

        public async Task<IActionResult> Index()
        {
            EmailConfigurationViewModel emailConfiguration = await
                _emailAdministrationService.GetEmailConfigurationAsync();

            return View(emailConfiguration);
        }

        [HttpPost]
        public async Task<IActionResult> Update(EmailConfigurationModel emailConfiguration)
        {
            await _emailAdministrationService.UpdateEmailConfigurationAsync(emailConfiguration);

            return RedirectToAction("Index", "Home");
        }
    }
}
