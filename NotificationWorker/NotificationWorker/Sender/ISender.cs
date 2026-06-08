namespace NotificationWorker.Sender;

public interface ISender
{
    Task SendAsync(string destination, string msg) {
        //TODO sent msg
    }
}
