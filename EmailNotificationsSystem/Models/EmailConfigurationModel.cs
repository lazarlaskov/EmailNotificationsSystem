using System.ComponentModel.DataAnnotations;

namespace EmailNotificationsSystem.Models
{
    public class EmailConfigurationModel
    {
        [Required]
        public int ProtocolId { get; set; }
        [Required]
        public string HostName { get; set; }
        [Required]
        public int Port { get; set; }
        [Required]
        public string DefaultSender { get; set; }
        [Required]
        public int SecurityModeId { get; set; }
    }
}
