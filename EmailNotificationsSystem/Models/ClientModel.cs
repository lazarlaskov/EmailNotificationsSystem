using System.ComponentModel.DataAnnotations;

namespace EmailNotificationsSystem.Models
{
    public class ClientModel
    {
        [Required]
        public int ClientId { get; set; }
        [Required]
        public string ClientName { get; set; }
    }
}
