using FarmKeeper.Enums;
using System.ComponentModel.DataAnnotations;

namespace FarmKeeper.Models.DTO
{
    public class UpdateAssignmentRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public Priority Priority { get; set; }
    }
}
