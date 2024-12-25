using FarmKeeper.Mappers;
using FarmKeeper.Models;
using FarmKeeper.Models.DTO;
using FarmKeeper.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FarmKeeper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StablesController : ControllerBase
    {
        private readonly IStableRepository stableRepository;
        private readonly IFarmRepository farmRepository;
        public StablesController(IStableRepository stableRepository, IFarmRepository farmRepository)
        {
            this.stableRepository = stableRepository;
            this.farmRepository = farmRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Owner,DatabaseAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var stableDomain = await stableRepository.GetAllAsync();
            var stableDto = stableDomain.Select(a => a.ToStableDto());

            return Ok(stableDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Owner,DatabaseAdmin")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var stableDomain = await stableRepository.GetByIdAsync(id);
            if (stableDomain == null)
            {
                return NotFound();
            }

            return Ok(stableDomain.ToStableDto());
        }

        [HttpPost("{farmId}")]
        [Authorize(Roles = "Owner,DatabaseAdmin")]
        public async Task<IActionResult> Create([FromRoute] Guid farmId, [FromBody] AddStableRequestDto addStableRequestDto)
        {
            if (ModelState.IsValid)
            {
                var farm = await farmRepository.GetByIdAsync(farmId);
                if (farm == null)
                {
                    return NotFound("Farm not found.");
                }

                var stableDomain = addStableRequestDto.ToStableFromCreate(farmId);

                stableDomain = await stableRepository.CreateAsync(stableDomain);
                var stableDto = stableDomain.ToStableDto();

                return CreatedAtAction(nameof(GetById), new { id = stableDomain.Id }, stableDto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Owner,DatabaseAdmin")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateStableRequestDto updateStableRequestDto)
        {
            if (ModelState.IsValid)
            {
                var farm = await farmRepository.GetByIdAsync(updateStableRequestDto.FarmId);
                if (farm == null)
                {
                    return NotFound($"Farm with ID {updateStableRequestDto.FarmId} not found.");
                }

                var stableDomain = await stableRepository.UpdateAsync(id, updateStableRequestDto.ToStableFromUpdate());
                if (stableDomain == null)
                {
                    return NotFound("Stable not found");
                }
                
                return Ok(stableDomain.ToStableDto());
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Owner,DatabaseAdmin")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var stableDomain = await stableRepository.DeleteAsync(id);
            if (stableDomain == null)
            {
                return NotFound("Stable does not exist");
            }

            return Ok(stableDomain.ToStableDto());
        }
    }
}
