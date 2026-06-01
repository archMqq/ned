using Confluent.Kafka;

namespace EntryPointAPI.Models
{

    public enum ChannelType
    {
        Email, 
        Telegram,
        SMS
    }

    public class Channel
    {
        public ChannelType Type;
        public string Address;
    }

    public class KafkaItem
    {
        public Guid Id;
        public int UserId { get; }
        public Channel[] Channels { get; }

        public KafkaItem(int userId, Channel[] channels)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Channels = channels;
        }
    }
}
