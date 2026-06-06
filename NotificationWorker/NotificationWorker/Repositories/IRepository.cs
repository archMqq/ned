namespace NotificationWorker.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<int> SaveAsync(T entity);
        Task<T> GetByIdASync(int id);
    }
}
