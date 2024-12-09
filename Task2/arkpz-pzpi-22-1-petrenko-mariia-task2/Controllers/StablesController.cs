using Models;
using Models.DTO;
using Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StablesController : ControllerBase
    {
        private readonly IStableRepository stableRepository;
        public StablesController(IStableRepository stableRepository)
        {
            this.stableRepository = stableRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stableDomain = await stableRepository.GetAllAsync();
            var stableDto = new List<StableDto>();
            foreach (var stable in stableDomain)
            {
                stableDto.Add(new StableDto()
                {
                    Id = stable.Id,
                    MinFeedLevel = stable.MinFeedLevel,
                    CurrentFeedLevel = stable.CurrentFeedLevel,
                    DateTimeOfUpdate = stable.DateTimeOfUpdate,
                    FarmId = stable.FarmId,
                });
            }
            return Ok(stableDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var stableDomain = await stableRepository.GetByIdAsync(id);
            if (stableDomain == null)
            {
                return NotFound();
            }
            var stableDto = new StableDto
            {
                Id = stableDomain.Id,
                MinFeedLevel = stableDomain.MinFeedLevel,
                CurrentFeedLevel = stableDomain.CurrentFeedLevel,
                DateTimeOfUpdate = stableDomain.DateTimeOfUpdate,
                FarmId = stableDomain.FarmId,

            };
            return Ok(stableDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddStableRequestDto addStableRequestDto)
        {
            if (ModelState.IsValid)
            {
                var stableDomain = new Stable
                {
                    MinFeedLevel = addStableRequestDto.MinFeedLevel,
                    CurrentFeedLevel = addStableRequestDto.CurrentFeedLevel,
                    DateTimeOfUpdate = addStableRequestDto.DateTimeOfUpdate,
                    FarmId = addStableRequestDto.FarmId,
                };

                stableDomain = await stableRepository.CreateAsync(stableDomain);
                var stableDto = new Stable
                {
                    MinFeedLevel = stableDomain.MinFeedLevel,
                    CurrentFeedLevel = stableDomain.CurrentFeedLevel,
                    DateTimeOfUpdate = stableDomain.DateTimeOfUpdate,
                    FarmId = stableDomain.FarmId,
                };

                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateStableRequestDto updateStableRequestDto)
        {
            if (ModelState.IsValid)
            {
                var stableDomain = new Stable
                {
                    MinFeedLevel = updateStableRequestDto.MinFeedLevel,
                    CurrentFeedLevel = updateStableRequestDto.CurrentFeedLevel,
                    DateTimeOfUpdate = updateStableRequestDto.DateTimeOfUpdate,
                    FarmId = updateStableRequestDto.FarmId,
                };
                stableDomain = await stableRepository.UpdateAsync(id, stableDomain);
                if (stableDomain == null)
                {
                    return NotFound();
                }
                var stableDto = new Stable
                {
                    MinFeedLevel = stableDomain.MinFeedLevel,
                    CurrentFeedLevel = stableDomain.CurrentFeedLevel,
                    DateTimeOfUpdate = stableDomain.DateTimeOfUpdate,
                    FarmId = stableDomain.FarmId,
                };
                return Ok(stableDto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var stableDomain = await stableRepository.DeleteAsync(id);
            if (stableDomain == null)
            {
                return NotFound();
            }
            var stableDto = new StableDto
            {
                Id = stableDomain.Id,
                MinFeedLevel = stableDomain.MinFeedLevel,
                CurrentFeedLevel = stableDomain.CurrentFeedLevel,
                DateTimeOfUpdate = stableDomain.DateTimeOfUpdate,
                FarmId = stableDomain.FarmId,
            };

            return Ok(stableDto);
        }
    }
}
