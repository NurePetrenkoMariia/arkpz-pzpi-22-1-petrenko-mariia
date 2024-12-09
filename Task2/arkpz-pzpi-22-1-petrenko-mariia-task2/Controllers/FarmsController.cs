using Models;
using Models.DTO;
using Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FarmsController : ControllerBase
    {
        private readonly IFarmRepository farmRepository;
        public FarmsController(IFarmRepository farmRepository)
        {
            this.farmRepository = farmRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var farmsDomain = await farmRepository.GetAllAsync();
            var farmsDto = new List<FarmDto>();
            foreach (var farm in farmsDomain)
            {
                farmsDto.Add(new FarmDto()
                {
                    Id = farm.Id,
                    Name = farm.Name,
                    Country = farm.Country,
                    City = farm.City,
                    Street = farm.Street,
                    OwnerId = farm.OwnerId,

                });
            }
            return Ok(farmsDto);
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
            var farmDto = new FarmDto
            {
                Id = farmDomain.Id,
                Name = farmDomain.Name,
                Country = farmDomain.Country,
                City = farmDomain.City,
                Street = farmDomain.Street,
                OwnerId = farmDomain.OwnerId,

            };
            return Ok(farmDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddFarmRequestDto addFarmRequestDto)
        {
            var farmDomain = new Farm
            {
                Name = addFarmRequestDto.Name,
                Country = addFarmRequestDto.Country,
                City = addFarmRequestDto.City,
                Street = addFarmRequestDto.Street,
                OwnerId = addFarmRequestDto.OwnerId,
            };

            farmDomain = await farmRepository.CreateAsync(farmDomain);
            var farmDto = new Farm
            {
                Name = farmDomain.Name,
                Country = farmDomain.Country,
                City = farmDomain.City,
                Street = farmDomain.Street,
                OwnerId = farmDomain.OwnerId,
            };

            return Ok();
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateFarmRequestDto updateFarmRequestDto)
        {
            var farmDomain = new Farm
            {
                Name = updateFarmRequestDto.Name,
                Country = updateFarmRequestDto.Country,
                City = updateFarmRequestDto.City,
                Street = updateFarmRequestDto.Street,
                OwnerId = updateFarmRequestDto.OwnerId,
            };

            farmDomain = await farmRepository.UpdateAsync(id, farmDomain);
            if (farmDomain == null)
            {
                return NotFound();
            }
            var farmDto = new Farm
            {
                Name = farmDomain.Name,
                Country = farmDomain.Country,
                City = farmDomain.City,
                Street = farmDomain.Street,
                OwnerId = farmDomain.OwnerId,
            };
            return Ok(farmDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var farmDomain = await farmRepository.DeleteAsync(id);
            if (farmDomain == null)
            {
                return NotFound();
            }
            var farmDto = new FarmDto
            {
                Id = farmDomain.Id,
                Name = farmDomain.Name,
                Country = farmDomain.Country,
                City = farmDomain.City,
                Street = farmDomain.Street,
                OwnerId = farmDomain.OwnerId,
            };

            return Ok(farmDto);
        }
    }
}
