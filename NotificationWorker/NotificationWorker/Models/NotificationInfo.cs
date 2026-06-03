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
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string? Message { get; set; }
        public MessageStatus Status { get; set; }
        public DateTime SentAt { get; set; }
    }
}
