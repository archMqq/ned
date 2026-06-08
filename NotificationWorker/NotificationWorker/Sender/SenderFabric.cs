namespace NotificationWorker.Sender;

public class SenderFabric
{
    public Task<List<ISender>> CreateSenders(string[] senderTypes) {
        var res = new List<ISender> ();
        foreach (var item in senderTypes)
        {
            switch(item){
                case "Email":{
                    res.Add(new EmailSender);
                    break;
                }
            }
        }

        return Task.FromResult(res)
    }
}
