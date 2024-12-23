using FarmKeeper.Mappers;
using FarmKeeper.Models.DTO;
using FarmKeeper.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FarmKeeper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTaskController : ControllerBase
    {
        private readonly IUserTaskRepository userTaskRepository;
        public UserTaskController(IUserTaskRepository userTaskRepository)
        {
            this.userTaskRepository = userTaskRepository;
        }

        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetAll()
        {
            var userTaskDomain = await userTaskRepository.GetAllAsync();
            var userTaskDto = userTaskDomain.Select(a => a.ToUserTaskDto());
            return Ok(userTaskDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var userTaskDomain = await userTaskRepository.GetByIdAsync(id);
            if (userTaskDomain == null)
            {
                return NotFound();
            }

            return Ok(userTaskDomain.ToUserTaskDto());
        }
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> Create([FromBody] AddUserTaskRequestDto addUserTaskRequestDto)
        {
            var userTaskDomain = addUserTaskRequestDto.ToUserTaskFromCreate();

            userTaskDomain = await userTaskRepository.CreateAsync(userTaskDomain);

            var userTaskDto = userTaskDomain.ToUserTaskDto();

            return CreatedAtAction(nameof(GetById), new { id = userTaskDomain.Id }, userTaskDto);
        }
    }
}
