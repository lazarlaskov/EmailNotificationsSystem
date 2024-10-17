using System.ComponentModel.DataAnnotations;

namespace EmailNotificationsSystem.Models
{
    public class EmailModel
    {
        [Required]
        public int ClientId { get; set; }
        [Required]
        public int TemplateId { get; set; }
        [Required]
        public string MarketingData { get; set; }
    }
}
