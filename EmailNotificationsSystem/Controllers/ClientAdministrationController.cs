using EmailNotificationsSystem.Models;
using EmailNotificationsSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmailNotificationsSystem.Controllers
{
    public class ClientAdministrationController : Controller
    {
        private readonly IClientPreparationService _clientPreparationService;
        private readonly IClientManagementService _clientManagementService;

        public ClientAdministrationController(
            IClientPreparationService clientPreparationService,
            IClientManagementService clientManagementService)
        {
            _clientPreparationService = clientPreparationService;
            _clientManagementService = clientManagementService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<ClientModel> clients = await _clientPreparationService.PrepareClientsAsync();
            await _clientPreparationService.PrepareClientsDataAsync();
            return View(clients);
        }

        [HttpGet]
        public async Task<IActionResult> EditClient(int clientId)
        {
            ClientDataModel clientData = await _clientManagementService.GetClientDataAsync(clientId);
            return View(clientData);
        }

        [HttpPost]
        public async Task<IActionResult> EditClient(ClientDataModel clientData)
        {
            await _clientManagementService.UpdateClientDataAsync(clientData);
            return RedirectToAction("Index");
        }
    }
}
