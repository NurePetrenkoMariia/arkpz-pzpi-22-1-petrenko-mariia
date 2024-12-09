using Models;
using Models.DTO;
using Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
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
        public async Task<IActionResult> GetAll()
        {
            var assignmentsDomain = await assignmentRepository.GetAllAsync();
            var assignmentsDto = new List<AssignmentDto>();
            foreach (var assignment in assignmentsDomain)
            {
                assignmentsDto.Add(new AssignmentDto()
                {
                    Id = assignment.Id,
                    Name = assignment.Name,
                    Description = assignment.Description,
                    NumberOfParticipants = assignment.NumberOfParticipants,
                    StatusId = assignment.StatusId,
                    PriorityId = assignment.PriorityId,
                });
            }
            return Ok(assignmentsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var assignmentDomain = await assignmentRepository.GetByIdAsync(id);
            if (assignmentDomain == null)
            {
                return NotFound();
            }
            var assignmentDto = new AssignmentDto
            {
                Id = assignmentDomain.Id,
                Name = assignmentDomain.Name,
                Description = assignmentDomain.Description,
                NumberOfParticipants = assignmentDomain.NumberOfParticipants,
                StatusId = assignmentDomain.StatusId,
                PriorityId = assignmentDomain.PriorityId,
            };
            return Ok(assignmentDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddAssignmentRequestDto addAssignmentRequestDto)
        {
            if (ModelState.IsValid)
            {
                var assignmentDomain = new Assignment
                {
                    Name = addAssignmentRequestDto.Name,
                    Description = addAssignmentRequestDto.Description,
                    NumberOfParticipants = addAssignmentRequestDto.NumberOfParticipants,
                    StatusId = addAssignmentRequestDto.StatusId,
                    PriorityId = addAssignmentRequestDto.PriorityId,
                };

                assignmentDomain = await assignmentRepository.CreateAsync(assignmentDomain);
                var assignmentDto = new Assignment
                {
                    Name = assignmentDomain.Name,
                    Description = assignmentDomain.Description,
                    NumberOfParticipants = assignmentDomain.NumberOfParticipants,
                    StatusId = assignmentDomain.StatusId,
                    PriorityId = assignmentDomain.PriorityId,
                };

                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateAssignmentRequestDto updateAssignmentRequestDto)
        {
            if (ModelState.IsValid)
            {
                var assignmentDomain = new Assignment
                {
                    Name = updateAssignmentRequestDto.Name,
                    Description = updateAssignmentRequestDto.Description,
                    NumberOfParticipants = updateAssignmentRequestDto.NumberOfParticipants,
                    StatusId = updateAssignmentRequestDto.StatusId,
                    PriorityId = updateAssignmentRequestDto.PriorityId,
                };
                assignmentDomain = await assignmentRepository.UpdateAsync(id, assignmentDomain);
                if (assignmentDomain == null)
                {
                    return NotFound();
                }
                var assignmentDto = new Assignment
                {
                    Name = assignmentDomain.Name,
                    Description = assignmentDomain.Description,
                    NumberOfParticipants = assignmentDomain.NumberOfParticipants,
                    StatusId = assignmentDomain.StatusId,
                    PriorityId = assignmentDomain.PriorityId,
                };
                return Ok(assignmentDto);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var assignmentDomain = await assignmentRepository.DeleteAsync(id);
            if (assignmentDomain == null)
            {
                return NotFound();
            }
            var assignmentDto = new AssignmentDto
            {
                Id = assignmentDomain.Id,
                Name = assignmentDomain.Name,
                Description = assignmentDomain.Description,
                NumberOfParticipants = assignmentDomain.NumberOfParticipants,
                StatusId = assignmentDomain.StatusId,
                PriorityId = assignmentDomain.PriorityId,
            };

            return Ok(assignmentDto);
        }

    }
}
