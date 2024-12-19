using Models.DTO;
using Models;

namespace Mappers
{
    public static class AssignmentMapper
    {
        public static AssignmentDto ToAssignmentDto(this Assignment assignmentDomain)
        {
            return new AssignmentDto
            {
                Id = assignmentDomain.Id,
                Name = assignmentDomain.Name,
                Description = assignmentDomain.Description,
                Status = assignmentDomain.Status,
                Priority = assignmentDomain.Priority,
            };
        }

        public static Assignment ToAssignmentFromCreate(this AddAssignmentRequestDto addAssignmentRequestDto)
        {
            return new Assignment
            {
                Name = addAssignmentRequestDto.Name,
                Description = addAssignmentRequestDto.Description,
                Status = addAssignmentRequestDto.Status,
                Priority = addAssignmentRequestDto.Priority,
            };
        }

        public static Assignment ToAssignmentFromUpdate(this UpdateAssignmentRequestDto updateAssignmentRequestDto)
        {
            return new Assignment
            {
                Name = updateAssignmentRequestDto.Name,
                Description = updateAssignmentRequestDto.Description,
                Status = updateAssignmentRequestDto.Status,
                Priority = updateAssignmentRequestDto.Priority,
            };
        }
    }
}
