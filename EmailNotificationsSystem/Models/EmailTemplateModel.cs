using System.ComponentModel.DataAnnotations;

namespace EmailNotificationsSystem.Models
{
    public class EmailTemplateModel
    {
        [Required]
        public int TemplateId { get; set; }
        [Required]
        public string TemplateName { get; set; }
        [Required]
        public string TemplateContent { get; set; }
    }
}
