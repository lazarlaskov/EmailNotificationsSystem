using System.ComponentModel.DataAnnotations;

namespace EmailNotificationsSystem.Models
{
    public class ClientDataModel
    {
        [Required]
        public int ClientId { get; set; }
        [Required]
        public int TemplateId { get; set; }
        [Required]
        public string TemplateName { get; set; }
        [Required]
        public string MarketingData { get; set; }
    }
}
