using FarmKeeper.Models;
using FarmKeeper.Models.DTO;
using FarmKeeper.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FarmKeeper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly IAnimalRepository animalRepository;
        public AnimalsController(IAnimalRepository animalRepository)
        {
            this.animalRepository = animalRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var animalsDomain = await animalRepository.GetAllAsync();
            var animalsDTO = new List<AnimalDTO>();
            foreach (var animal in animalsDomain)
            {
                animalsDTO.Add(new AnimalDTO()
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
            return Ok(animalsDTO);
        }
    }
}
