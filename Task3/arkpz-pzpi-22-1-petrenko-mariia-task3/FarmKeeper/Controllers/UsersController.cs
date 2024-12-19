using FarmKeeper.Mappers;
using FarmKeeper.Models;
using FarmKeeper.Models.DTO;
using FarmKeeper.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarmKeeper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        public UsersController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var userDomain = await userRepository.GetAllAsync();
            var userDto = userDomain.Select(u => u.ToUserDto());
            return Ok(userDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var userDomain = await userRepository.GetByIdAsync(id);
            if (userDomain == null)
            {
                return NotFound();
            }
        
            return Ok(userDomain.ToUserDto());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] AddUserRequestDto addUserRequestDto)
        {
            var userDomain = addUserRequestDto.ToUserFromCreate();

            userDomain = await userRepository.CreateAsync(userDomain);

            var userDto = userDomain.ToUserDto();

            return CreatedAtAction(nameof(GetById), new { id = userDomain.Id }, userDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateUserRequestDto updateUserRequestDto)
        { 
            var userDomain = await userRepository.UpdateAsync(id, updateUserRequestDto.ToUserFromUpdate());
            if (userDomain == null)
            {
                return NotFound("User not found");
            }
           
            return Ok(userDomain.ToUserDto());
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var userDomain = await userRepository.DeleteAsync(id);
            if (userDomain == null)
            {
                return NotFound("User does not exist");
            }

            return Ok(userDomain.ToUserDto());
        }

    }
}
