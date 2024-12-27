using FarmKeeper.Data;
using FarmKeeper.Models;
using FarmKeeper.Models.DTO;
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

        public async Task<List<FeedLevelHistory>> GetAllAsync()
        {
            return await dbContext.FeedLevelHistory.ToListAsync();
        }

        public async Task<List<FeedLevelHistoryDtoForIoT>> GetFeedHistoryAsync(Guid stableId)
        {
            return await dbContext.FeedLevelHistory
                .Where(h => h.StableId == stableId)
                .OrderBy(h => h.Timestamp)
                .Select(h => new FeedLevelHistoryDtoForIoT
                {
                   FeedLevel = h.FeedLevel,
                   Timestamp = h.Timestamp
                })
                .ToListAsync(); 
        }

        public async Task MonitorFeedLevelAsync(FeedLevelHistory feedLevelHistory)
        {
            var stable = await stableRepository.GetByIdAsync(feedLevelHistory.StableId);

            if (stable == null)
            {
                throw new Exception("Stable not found.");
            }

            await dbContext.FeedLevelHistory.AddAsync(feedLevelHistory);

            if (feedLevelHistory.FeedLevel <= stable.MinFeedLevel)
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

            await dbContext.SaveChangesAsync();
        }   
    }
}
