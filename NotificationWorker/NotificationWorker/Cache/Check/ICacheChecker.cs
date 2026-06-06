namespace NotificationWorker.Cache.Check
{
    public interface ICacheChecker
    {
        Task<IResult> CheckInstanceAsync(Guid id);
    }
}
