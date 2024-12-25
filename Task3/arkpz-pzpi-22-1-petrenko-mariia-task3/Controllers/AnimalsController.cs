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
    public class AnimalsController : ControllerBase
    {
        private readonly IAnimalRepository animalRepository;
        private readonly IStableRepository stableRepository;
        public AnimalsController(IAnimalRepository animalRepository, IStableRepository stableRepository)
        {
            this.animalRepository = animalRepository;
            this.stableRepository = stableRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Owner,DatabaseAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var animalDomain = await animalRepository.GetAllAsync();
            var animalDto = animalDomain.Select(a => a.ToAnimalDto());
        
            return Ok(animalDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Owner,DatabaseAdmin")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var animalDomain = await animalRepository.GetByIdAsync(id);
            if (animalDomain == null)
            {
                return NotFound();
            }
           
            return Ok(animalDomain.ToAnimalDto());
        }

        [HttpPost("{stableId}")]
        [Authorize(Roles = "Owner,DatabaseAdmin")]
        public async Task<IActionResult> Create([FromRoute] Guid stableId, [FromBody] AddAnimalRequestDto addAnimalRequestDto)
        {
            var stable = await stableRepository.GetByIdAsync(stableId);
            if (stable == null)
            {
                return NotFound("Stable not found.");
            }

            var animalDomain = addAnimalRequestDto.ToAnimalFromCreate(stableId);

            animalDomain = await animalRepository.CreateAsync(animalDomain);

            var animalDto = animalDomain.ToAnimalDto();
            
            return CreatedAtAction(nameof(GetById), new { id = animalDomain.Id}, animalDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Owner,DatabaseAdmin")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateAnimalRequestDto updateAnimalRequestDto)
        {
            var stable = await stableRepository.GetByIdAsync(updateAnimalRequestDto.StableId);
            if (stable == null)
            {
                return NotFound("Stable not found.");
            }

            var animalDomain = await animalRepository.UpdateAsync(id, updateAnimalRequestDto.ToAnimalFromUpdate());
            
            if (animalDomain == null)
            {
                return NotFound("Animal not found");
            }

            return Ok(animalDomain.ToAnimalDto());
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Owner,DatabaseAdmin")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var animalDomain = await animalRepository.DeleteAsync(id);
            if (animalDomain == null)
            {
                return NotFound("Animal does not exist");
            }

            return Ok(animalDomain.ToAnimalDto());
        }

    }
}
