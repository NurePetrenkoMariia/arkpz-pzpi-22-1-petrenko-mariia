using FarmKeeper.Models.DTO;
using FarmKeeper.Repositories;
using FarmKeeper.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmKeeper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedMonitoringController : ControllerBase
    {
        private readonly IFeedMonitoringService feedMonitoringService;

        public FeedMonitoringController(IFeedMonitoringService feedMonitoringService)
        {
            this.feedMonitoringService = feedMonitoringService;
        }

        [HttpPost("update-feed-level")]
        [Authorize(Roles = "DatabaseAdmin")]
        public async Task<IActionResult> UpdateFeedLevel([FromBody] FeedUpdateDto feedUpdate)
        {
            if (feedUpdate == null)
            {
                return BadRequest("Feed update data is required.");
            }

            await feedMonitoringService.MonitorFeedLevelAsync(feedUpdate.StableId, feedUpdate.CurrentFeedLevel);
            return Ok("Feed level was updated");
        }
    }
}
