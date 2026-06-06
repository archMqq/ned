using Microsoft.EntityFrameworkCore;
using NotificationWorker.Database;
using NotificationWorker.Models;
using System.Data;

namespace NotificationWorker.Repositories
{
    public class NotificationRepository : IRepository<NotificationInfo>
    {
        readonly NtfContext _context;
        readonly ILogger<NotificationRepository> _logger;
        public NotificationRepository(NtfContext context, ILogger<NotificationRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> SaveAsync(NotificationInfo entity)
        {
            await _context.Notifications.AddAsync(entity);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException concEx)
            {
                _logger.LogError(concEx, "A concurrency error occurred while saving the notification.");
            }
            catch (DbUpdateException updEx)
            {
                _logger.LogError(updEx, "An error occurred while saving the notification.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while saving the notification.");
            }

            return entity.Id;
        }

        public async Task<NotificationInfo> GetByIdASync(int id)
        {
            var entity = await _context.Notifications.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                _logger.LogInformation("An attempt to obtain information on unknown notification.");
                throw new Exceptions.UnknownId("Attempt to obtain unknown notification");
            }

            return entity;
        }
    }
}
