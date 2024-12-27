using FarmKeeper.Mappers;
using FarmKeeper.Repositories;
using FarmKeeper.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FarmKeeper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedLevelHistoryController : ControllerBase
    {
        private readonly IFeedMonitoringService feedMonitoringService;
        public FeedLevelHistoryController(IFeedMonitoringService feedMonitoringService)
        {
            this.feedMonitoringService = feedMonitoringService;
        }

        //Метод для отримання даних ІоТ пристроєм
        [HttpGet("get-feed-history/{stableId}")]
        public async Task<IActionResult> GetFeedHistory(Guid stableId)
        {
            var history = await feedMonitoringService.GetFeedHistoryAsync(stableId);
            if (history == null || !history.Any())
            {
                return NotFound("No feed history found for the given stable ID.");
            }

            return Ok(history);
        }

        //Метод для отримання даних користувачем
        [HttpGet]
        [Authorize(Roles = "Owner,DatabaseAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var feedLevelHistoryDomain = await feedMonitoringService.GetAllAsync();
            var feedLevelHistoryDto = feedLevelHistoryDomain.Select(u => u.ToFeedLevelHistoryDto());
            return Ok(feedLevelHistoryDto);
        }

    }
}
