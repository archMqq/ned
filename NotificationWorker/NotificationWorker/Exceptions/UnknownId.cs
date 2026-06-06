namespace NotificationWorker.Exceptions
{
    public class UnknownId : ArgumentException
    {
        public UnknownId(string message) : base(message) 
        { }
    }
}
