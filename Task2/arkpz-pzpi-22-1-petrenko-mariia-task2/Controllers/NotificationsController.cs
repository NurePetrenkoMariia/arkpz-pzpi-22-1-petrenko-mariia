using Models;
using Models.DTO;
using Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationRepository notificationRepository;
        public NotificationsController(INotificationRepository notificationRepository)
        {
            this.notificationRepository = notificationRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var notificationsDomain = await notificationRepository.GetAllAsync();
            var notificationsDto = new List<NotificationDto>();
            foreach (var notification in notificationsDomain)
            {
                notificationsDto.Add(new NotificationDto()
                {
                    Id = notification.Id,
                    Title = notification.Title,
                    Text = notification.Text,
                    DateTimeCreated = notification.DateTimeCreated,
                    UserId = notification.UserId,
                });
            }
            return Ok(notificationsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var notificationDomain = await notificationRepository.GetByIdAsync(id);
            if (notificationDomain == null)
            {
                return NotFound();
            }
            var notificationDto = new NotificationDto
            {
                Id = notificationDomain.Id,
                Title = notificationDomain.Title,
                Text = notificationDomain.Text,
                DateTimeCreated = notificationDomain.DateTimeCreated,
                UserId = notificationDomain.UserId,

            };
            return Ok(notificationDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddNotificationRequestDto addNotificationRequestDto)
        {
            var notificationDomain = new Notification
            {
                Title = addNotificationRequestDto.Title,
                Text = addNotificationRequestDto.Text,
                DateTimeCreated = addNotificationRequestDto.DateTimeCreated,
                UserId = addNotificationRequestDto.UserId,
            };

            notificationDomain = await notificationRepository.CreateAsync(notificationDomain);
            var notificationDto = new Notification
            {
                Title = notificationDomain.Title,
                Text = notificationDomain.Text,
                DateTimeCreated = notificationDomain.DateTimeCreated,
                UserId = notificationDomain.UserId,
            };

            return Ok();
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateNotificationRequestDto updateNotificationRequestDto)
        {
            var notificationDomain = new Notification
            {
                Title = updateNotificationRequestDto.Title,
                Text= updateNotificationRequestDto.Text,
                DateTimeCreated = updateNotificationRequestDto.DateTimeCreated,
                UserId = updateNotificationRequestDto.UserId,
            };
            notificationDomain = await notificationRepository.UpdateAsync(id, notificationDomain);
            if (notificationDomain == null)
            {
                return NotFound();
            }
            var notificationDto = new Notification
            {
                Title = notificationDomain.Title,
                Text = notificationDomain.Text,
                DateTimeCreated = notificationDomain.DateTimeCreated,
                UserId = notificationDomain.UserId,
            };
            return Ok(notificationDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var notificationDomain = await notificationRepository.DeleteAsync(id);
            if (notificationDomain == null)
            {
                return NotFound();
            }
            var notificationDto = new NotificationDto
            {
                Id = notificationDomain.Id,
                Title = notificationDomain.Title,
                Text = notificationDomain.Text,
                DateTimeCreated = notificationDomain.DateTimeCreated,
                UserId = notificationDomain.UserId,
            };

            return Ok(notificationDto);
        }
    }
}
