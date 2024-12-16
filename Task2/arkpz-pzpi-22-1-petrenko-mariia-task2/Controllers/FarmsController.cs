using FarmKeeper.Mapper;
using FarmKeeper.Models;
using FarmKeeper.Models.DTO;
using FarmKeeper.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FarmKeeper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FarmsController : ControllerBase
    {
        private readonly IFarmRepository farmRepository;
        private readonly IUserRepository userRepository;
        public FarmsController(IFarmRepository farmRepository, IUserRepository userRepository)
        {
            this.farmRepository = farmRepository;
            this.userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var farmDomain = await farmRepository.GetAllAsync();
            var farmDto = farmDomain.Select(f => f.ToFarmDto());
            return Ok(farmDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var farmDomain = await farmRepository.GetByIdAsync(id);
            if (farmDomain == null)
            {
                return NotFound();
            }
            
            return Ok(farmDomain.ToFarmDto());
        }

        [HttpPost("{ownerId}")]
        public async Task<IActionResult> Create([FromRoute] Guid ownerId, [FromBody] AddFarmRequestDto addFarmRequestDto)
        {

            var user = await userRepository.GetByIdAsync(ownerId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            var farmDomain = addFarmRequestDto.ToFarmFromCreate(ownerId);

            farmDomain = await farmRepository.CreateAsync(farmDomain);

            var farmDto = farmDomain.ToFarmDto();
            return CreatedAtAction(nameof(GetById), new { id = farmDomain.Id }, farmDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateFarmRequestDto updateFarmRequestDto)
        {
            var user = await userRepository.GetByIdAsync(updateFarmRequestDto.OwnerId);
            if (user == null)
            {
                return NotFound($"User not found.");
            }

            var farmDomain = await farmRepository.UpdateAsync(id, updateFarmRequestDto.ToFarmFromUpdate());
            if (farmDomain == null)
            {
                return NotFound("Farm not found");
            }
            
            return Ok(farmDomain.ToFarmDto());
        }

        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var farmDomain = await farmRepository.DeleteAsync(id);
            if (farmDomain == null)
            {
                return NotFound("Farm does not exist");
            }

            return Ok(farmDomain.ToFarmDto());
        }
    }
}
