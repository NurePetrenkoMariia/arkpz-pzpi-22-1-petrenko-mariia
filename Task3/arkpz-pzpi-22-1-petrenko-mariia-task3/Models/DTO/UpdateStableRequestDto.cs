using System.ComponentModel.DataAnnotations;

namespace FarmKeeper.Models.DTO
{
    public class UpdateStableRequestDto
    {
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "MinFeedLevel must be a non-negative integer.")]
        public int MinFeedLevel { get; set; }

        //[Range(0, int.MaxValue, ErrorMessage = "CurrentFeedLevel must be a non-negative integer.")]
        //public int CurrentFeedLevel { get; set; }
        //public DateTime DateTimeOfUpdate { get; set; }
        [Required]
        public Guid FarmId { get; set; }
    }
}
