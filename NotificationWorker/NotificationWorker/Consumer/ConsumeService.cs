using Confluent.Kafka;

namespace NotificationWorker.Consumer
{
    public class ConsumeService : BackgroundService
    {
        readonly IConsumer<string, string> _consumer;
        readonly ILogger<ConsumeService> _logger;

        public ConsumeService(ConsumerConfig config, ILogger<ConsumeService> logger)
        {
            _consumer = new ConsumerBuilder<string, string>(new Confluent.Kafka.ConsumerConfig
            {
                BootstrapServers = config.BootstrapServer,
                GroupId = config.GroupId,
                AutoOffsetReset = config.AutoOffsetReset,
                EnableAutoCommit = config.AutoCommit
            }).Build();
            _consumer.Subscribe(config.Topic);
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumeResult = _consumer.Consume(stoppingToken);
                    if (consumeResult != null)
                    {
                        // TODO process the message
                    }
                    _consumer.Commit();
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Consumer is stopping due to cancellation.");
            }
            finally
            {
                _consumer.Close();
            }
            return Task.CompletedTask;
        }
    }
}
