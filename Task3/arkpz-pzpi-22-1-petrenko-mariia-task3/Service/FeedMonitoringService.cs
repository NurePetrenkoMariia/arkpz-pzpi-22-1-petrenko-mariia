using FarmKeeper.Data;
using FarmKeeper.Models;
using FarmKeeper.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FarmKeeper.Service
{
    public class FeedMonitoringService : IFeedMonitoringService
    {
        private readonly IStableRepository stableRepository;
        private readonly INotificationRepository notificationRepository;
        private readonly FarmKeeperDbContext dbContext;
        public FeedMonitoringService(IStableRepository stableRepository, 
            INotificationRepository notificationRepository, 
            FarmKeeperDbContext dbContext)
        {
            this.stableRepository = stableRepository;
            this.notificationRepository = notificationRepository;
            this.dbContext = dbContext;
        }

        public async Task MonitorFeedLevelAsync(Guid stableId, int currentFeedLevel)
        {
            var stable = await stableRepository.GetByIdAsync(stableId);

            if (stable == null)
            {
                throw new Exception("Stable not found.");
            }

            if (currentFeedLevel <= stable.MinFeedLevel)
            {

                var userId = stable.Farm.OwnerId;
                var notification = new Notification
                {
                    Id = Guid.NewGuid(),
                    Title = "Low feed level!",
                    Text = $"Low feed level at stable with ID {stable.Id}.",
                    DateTimeCreated = DateTime.UtcNow,
                    UserId = userId,
                };

                await notificationRepository.CreateAsync(notification);
            }

            stable.CurrentFeedLevel = currentFeedLevel;
            stable.DateTimeOfUpdate = DateTime.UtcNow;
            await dbContext.SaveChangesAsync();
        }   
    }
}
