using EmailNotificationsSystem.Models;
using EmailNotificationsSystem.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using MSMQ.Messaging;
using Newtonsoft.Json;

namespace EmailNotificationsSystem.Services
{
    public class MessageQueueService : IMessageQueueService
    {
        private readonly ILogger<MessageQueueService> _logger;
        private readonly IDistributedCache _redisCache;
        private readonly IEmailManagementService _emailManagementService;

        public MessageQueueService(ILogger<MessageQueueService> logger,
            IDistributedCache redisCache,
            IEmailManagementService emailManagementService)
        {
            _logger = logger;
            _redisCache = redisCache;
            _emailManagementService = emailManagementService;
        }

        public async Task SendEmailMessagesAsync(
            IEnumerable<EmailModel> emailMessages)
        {
            MessageQueue messageQueue = new MessageQueue(".\\private$\\emails");

            foreach (EmailModel emailMessage in emailMessages)
            {
                messageQueue.Send(emailMessage);
            }
        }

        public void InitializeMessageQueue(string queuePath)
        {
            string queueName = @".\private$\" + queuePath;

            if (!MessageQueue.Exists(queueName))
            {
                MessageQueue.Create(queueName);
            }

            MessageQueue msQueue = new MessageQueue(queueName);
            msQueue.ReceiveCompleted += QueueMessageReceived;
            msQueue.BeginReceive();
        }

        private void QueueMessageReceived(object source, ReceiveCompletedEventArgs args)
        {
            MessageQueue msQueue = (MessageQueue)source;

            Message message = null;
            message = msQueue.EndReceive(args.AsyncResult);

            message.Formatter = new XmlMessageFormatter(new Type[] { typeof(EmailModel) });
            EmailModel email = (EmailModel)message.Body;

            _emailManagementService.SaveEmailAsync(email);

            msQueue.BeginReceive();
        }
    }
}
