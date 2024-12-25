using FarmKeeper.Enums;
using FarmKeeper.Mappers;
using FarmKeeper.Models;
using FarmKeeper.Models.DTO;
using FarmKeeper.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarmKeeper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        private readonly IAssignmentRepository assignmentRepository;
        public AssignmentsController(IAssignmentRepository assignmentRepository)
        {
            this.assignmentRepository = assignmentRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Owner,DatabaseAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var assignmentDomain = await assignmentRepository.GetAllAsync();
            var assignmentDto = assignmentDomain.Select(a => a.ToAssignmentDto());
            return Ok(assignmentDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Owner,DatabaseAdmin")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var assignmentDomain = await assignmentRepository.GetByIdAsync(id);
            if (assignmentDomain == null)
            {
                return NotFound();
            }

            return Ok(assignmentDomain.ToAssignmentDto());
        }

        [HttpPost]
        [Authorize(Roles = "Owner,DatabaseAdmin")]
        public async Task<IActionResult> Create([FromBody] AddAssignmentRequestDto addAssignmentRequestDto)
        {
            var assignmentDomain = addAssignmentRequestDto.ToAssignmentFromCreate();

            assignmentDomain = await assignmentRepository.CreateAsync(assignmentDomain);

            var assignmentDto = assignmentDomain.ToAssignmentDto();

            return CreatedAtAction(nameof(GetById), new { id = assignmentDomain.Id }, assignmentDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Owner,DatabaseAdmin")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateAssignmentRequestDto updateAssignmentRequestDto)
        {
            var assignmentDomain = await assignmentRepository.UpdateAsync(id, updateAssignmentRequestDto.ToAssignmentFromUpdate());
            if (assignmentDomain == null)
            {
                return NotFound("Assignment not found");
            }

            return Ok(assignmentDomain.ToAssignmentDto());
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Owner,DatabaseAdmin")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var assignmentDomain = await assignmentRepository.DeleteAsync(id);
            if (assignmentDomain == null)
            {
                return NotFound("Assignment does not exist");
            }

            return Ok(assignmentDomain.ToAssignmentDto());
        }
    }
}
