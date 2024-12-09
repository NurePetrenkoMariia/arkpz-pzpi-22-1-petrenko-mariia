using Models;
using Models.DTO;
using Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Controllers
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
        public async Task<IActionResult> GetAll()
        {
            var usersDomain = await userRepository.GetAllAsync();
            var usersDto = new List<UserDto>();
            foreach (var user in usersDomain)
            {
                usersDto.Add(new UserDto()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DateOfBirth = user.DateOfBirth,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    PasswordHash = user.PasswordHash,
                    FarmId = user.FarmId,
                    RoleId = user.RoleId,
                });
            }
            return Ok(usersDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var userDomain = await userRepository.GetByIdAsync(id);
            if (userDomain == null)
            {
                return NotFound();
            }
            var userDto = new UserDto
            {
                Id = userDomain.Id,
                FirstName = userDomain.FirstName,
                LastName = userDomain.LastName,
                DateOfBirth = userDomain.DateOfBirth,
                PhoneNumber = userDomain.PhoneNumber,
                Email = userDomain.Email,
                PasswordHash = userDomain.PasswordHash,
                FarmId = userDomain.FarmId,
                RoleId = userDomain.RoleId,
            };
            return Ok(userDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddUserRequestDto addUserRequestDto)
        {
            var userDomain = new User
            {
                FirstName = addUserRequestDto.FirstName,
                LastName = addUserRequestDto.LastName,
                DateOfBirth = addUserRequestDto.DateOfBirth,
                PhoneNumber = addUserRequestDto.PhoneNumber,
                Email = addUserRequestDto.Email,
                PasswordHash = addUserRequestDto.PasswordHash,
                FarmId= addUserRequestDto.FarmId,
                RoleId= addUserRequestDto.RoleId,
            };

            userDomain = await userRepository.CreateAsync(userDomain);
            var userDto = new User
            {
                FirstName = userDomain.FirstName,
                LastName = userDomain.LastName,
                DateOfBirth = userDomain.DateOfBirth,
                PhoneNumber = userDomain.PhoneNumber,
                Email = userDomain.Email,
                PasswordHash = userDomain.PasswordHash,
                FarmId = userDomain.FarmId,
                RoleId = userDomain.RoleId,
            };

            return Ok();
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateUserRequestDto updateUserRequestDto)
        {
            var userDomain = new User
            {
                FirstName = updateUserRequestDto.FirstName,
                LastName = updateUserRequestDto.LastName,
                DateOfBirth = updateUserRequestDto.DateOfBirth,
                PhoneNumber = updateUserRequestDto.PhoneNumber,
                Email = updateUserRequestDto.Email,
                PasswordHash = updateUserRequestDto.PasswordHash,
                FarmId = updateUserRequestDto.FarmId,
                RoleId = updateUserRequestDto.RoleId,
            };

            userDomain = await userRepository.UpdateAsync(id, userDomain);
            if (userDomain == null)
            {
                return NotFound();
            }
            var userDto = new User
            {
                FirstName = userDomain.FirstName,
                LastName = userDomain.LastName,
                DateOfBirth = userDomain.DateOfBirth,
                PhoneNumber = userDomain.PhoneNumber,
                Email = userDomain.Email,
                PasswordHash = userDomain.PasswordHash,
                FarmId = userDomain.FarmId,
                RoleId = userDomain.RoleId,
            };
            return Ok(userDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var userDomain = await userRepository.DeleteAsync(id);
            if (userDomain == null)
            {
                return NotFound();
            }
            var userDto = new UserDto
            {
                FirstName = userDomain.FirstName,
                LastName = userDomain.LastName,
                DateOfBirth = userDomain.DateOfBirth,
                PhoneNumber = userDomain.PhoneNumber,
                Email = userDomain.Email,
                PasswordHash = userDomain.PasswordHash,
                FarmId = userDomain.FarmId,
                RoleId = userDomain.RoleId,
            };

            return Ok(userDto);
        }

    }
}
