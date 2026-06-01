using Confluent.Kafka;
using EntryPointAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EntryPointAPI.Kafka
{
    public interface INotificationProducer
    {
        void Produce(KafkaItem item);
    }

    public class NotificationProducer : INotificationProducer
    {
        private IProducer<string, string> _producer;
        private string _topic;
        private ILogger<NotificationProducer> _logger;

        public NotificationProducer(NotificationConfig config, ILogger<NotificationProducer> logger)
        {
            var prodConfig = new ProducerConfig
            {
                BootstrapServers = config.BootstrapServers
            };
            _topic = config.Topic;
            _producer = new ProducerBuilder<string, string>(prodConfig).Build();
            _logger = logger;
        }

        public void Produce(KafkaItem item)
        {
            var text = JsonSerializer.Serialize(item);
            var message = new Message<string, string>
            {
                Key = item.Id.ToString(),
                Value = text
            };

            _producer.Produce(_topic, message, (deliveryReport) =>
            {
                if (deliveryReport.Error.IsError)
                {
                    _logger.LogError($"Failed to deliver message: {deliveryReport.Error.Reason}");
                }
            });
        }
    }
}
