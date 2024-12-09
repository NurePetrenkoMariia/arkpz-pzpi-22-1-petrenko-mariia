using Models;
using Models.DTO;
using Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
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
        public async Task<IActionResult> GetAll()
        {
            var animalsDomain = await animalRepository.GetAllAsync();
            var animalsDto = new List<AnimalDto>();
            foreach (var animal in animalsDomain)
            {
                animalsDto.Add(new AnimalDto()
                {
                    Id = animal.Id,
                    Name = animal.Name,
                    Species = animal.Species,
                    Breed = animal.Breed,
                    DateOfBirth = animal.DateOfBirth,
                    Sex = animal.Sex,
                    FarmId = animal.FarmId,
                    StableId = animal.StableId,

                });
            }
            return Ok(animalsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var animalDomain = await animalRepository.GetByIdAsync(id);
            if (animalDomain == null)
            {
                return NotFound();
            }
            var animalDto = new AnimalDto
            {
                Id = animalDomain.Id,
                Name = animalDomain.Name,
                Species = animalDomain.Species,
                Breed = animalDomain.Breed,
                DateOfBirth = animalDomain.DateOfBirth,
                Sex = animalDomain.Sex,
                FarmId = animalDomain.FarmId,
                StableId = animalDomain.StableId,

            };
            return Ok(animalDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddAnimalRequestDto addAnimalRequestDto)
        {
            var stable = await stableRepository.GetByIdAsync(addAnimalRequestDto.StableId);
            if (stable == null)
            {
                return NotFound($"Stable with ID {addAnimalRequestDto.StableId} not found.");
            }

            var animalDomain = new Animal
            {
                Name = addAnimalRequestDto.Name,
                Species = addAnimalRequestDto.Species,
                Breed = addAnimalRequestDto.Breed,
                DateOfBirth = addAnimalRequestDto.DateOfBirth,
                Sex = addAnimalRequestDto.Sex,
                StableId = addAnimalRequestDto.StableId,
                FarmId = stable.FarmId,
            };

            animalDomain = await animalRepository.CreateAsync(animalDomain);
            var animalDto = new Animal
            {
                Name = animalDomain.Name,
                Species = animalDomain.Species,
                Breed = animalDomain.Breed,
                DateOfBirth = animalDomain.DateOfBirth,
                Sex = animalDomain.Sex,
                StableId = animalDomain.StableId,
                FarmId = animalDomain.FarmId,
            };

            return Ok();
        }

        [HttpPut]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateAnimalRequestDto updateAnimalRequestDto)
        {
            var stable = await stableRepository.GetByIdAsync(updateAnimalRequestDto.StableId);
            if (stable == null)
            {
                return NotFound($"Stable with ID {updateAnimalRequestDto.StableId} not found.");
            }
            var animalDomain = new Animal
            {
                Name = updateAnimalRequestDto.Name,
                Species = updateAnimalRequestDto.Species,
                Breed = updateAnimalRequestDto.Breed,
                DateOfBirth = updateAnimalRequestDto.DateOfBirth,
                Sex = updateAnimalRequestDto.Sex,
                StableId = updateAnimalRequestDto.StableId,
                FarmId = stable.FarmId,
            };
            animalDomain = await animalRepository.UpdateAsync(id, animalDomain);
            if (animalDomain == null)
            {
                return NotFound();
            }
            var animalDto = new Animal
            {
                Name = animalDomain.Name,
                Species = animalDomain.Species,
                Breed = animalDomain.Breed,
                DateOfBirth = animalDomain.DateOfBirth,
                Sex = animalDomain.Sex,
                StableId = animalDomain.StableId,
                FarmId = animalDomain.FarmId,
            };
            return Ok(animalDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var animalDomain = await animalRepository.DeleteAsync(id);
            if (animalDomain == null)
            {
                return NotFound();
            }
            var animalDto = new AnimalDto
            {
                Id = animalDomain.Id,
                Name = animalDomain.Name,
                Species = animalDomain.Species,
                Breed = animalDomain.Breed,
                DateOfBirth = animalDomain.DateOfBirth,
                Sex = animalDomain.Sex,
                FarmId = animalDomain.FarmId,
                StableId = animalDomain.StableId,
            };

            return Ok(animalDto);
        }

     }
}
