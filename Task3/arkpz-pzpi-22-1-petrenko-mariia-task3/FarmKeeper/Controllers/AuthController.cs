﻿using FarmKeeper.Enums;
using FarmKeeper.Models;
using FarmKeeper.Models.DTO;
using FarmKeeper.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FarmKeeper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenService tokenService;
        public AuthController(IUserRepository userRepository, ITokenService tokenService)
        {
            this.userRepository = userRepository;
            this.tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AddAdminUserRequestDto request)
        {
            var existingUser = await userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return BadRequest("User with this email already exists.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);

            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                PasswordHash = passwordHash,
                Role = UserRole.Admin, 
                Farms = new List<Farm>() 
            };

            await userRepository.CreateAsync(user);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await userRepository.GetByEmailAsync(request.Email);
            if (existingUser == null)
            {
                return Unauthorized("Invalid email");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, existingUser.PasswordHash))
            {
                return Unauthorized("Invalid email or/and password");
            }

            var token = tokenService.CreateToken(existingUser);
            return Ok(token);
        }
    }
}
