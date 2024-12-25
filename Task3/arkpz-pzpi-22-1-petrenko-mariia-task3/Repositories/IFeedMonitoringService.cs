namespace FarmKeeper.Repositories
{
    public interface IFeedMonitoringService
    {
        Task MonitorFeedLevelAsync(Guid stableId, int currentFeedLevel);
    }
}
