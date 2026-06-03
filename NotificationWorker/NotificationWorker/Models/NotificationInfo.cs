namespace NotificationWorker.Models
{
    public enum MessageStatus
    {
        Sent,
        Failed,
        Pending
    }

    public class NotificationInfo
    {
        public int Id { get; set; }
        public Guid KafkaId { get; set; }
        public int UserId { get; set; }
        public string? Message { get; set; }
        public MessageStatus Status { get; set; }
        public DateTime SentAt { get; set; }
    }
}
