using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmailNotificationsSystem.Models
{
    public class EmailConfigurationViewModel : EmailConfigurationModel
    {
        public IEnumerable<SelectListItem> Protocols { get; set; }
        public IEnumerable<SelectListItem> SecurityModes { get; set; }
    }
}
