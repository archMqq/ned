namespace NotificationWorker.Cache.Save
{
    public interface ICacheSaver
    {
        Task<IResult> SaveInstanceAsync(Guid id, string data);
    }
}
