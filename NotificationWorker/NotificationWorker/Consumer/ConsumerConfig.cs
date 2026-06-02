using Confluent.Kafka;

namespace NotificationWorker.Consumer
{
    public class ConsumerConfig
    {
        public string BootstrapServer;
        public string GroupId;
        public string Topic;
        public bool AutoCommit;
        public AutoOffsetReset AutoOffsetReset;
    }
}
