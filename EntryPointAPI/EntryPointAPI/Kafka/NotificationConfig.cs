namespace EntryPointAPI.Kafka
{
    public class NotificationConfig
    {
        public string BootstrapServers { get; init; }
        public string Topic { get; init; }

        public NotificationConfig(string bootstrapServers, string topic)
        {
            BootstrapServers = bootstrapServers;
            Topic = topic;
        }
    }
}
