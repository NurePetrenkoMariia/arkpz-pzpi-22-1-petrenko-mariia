using System.ComponentModel.DataAnnotations;

namespace Models.DTO
{
    public class AddAssignmentRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Number of participants must be a non-negative integer.")]
        public int NumberOfParticipants { get; set; }

        public Guid StatusId { get; set; }
        public Guid PriorityId { get; set; }
    }
}
