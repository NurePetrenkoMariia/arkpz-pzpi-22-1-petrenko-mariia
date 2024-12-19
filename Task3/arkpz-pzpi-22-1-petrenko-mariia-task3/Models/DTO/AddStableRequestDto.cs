using System.ComponentModel.DataAnnotations;

namespace Models.DTO
{
    public class AddStableRequestDto
    {
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "MinFeedLevel must be a non-negative integer.")]
        public int MinFeedLevel { get; set; }
    }
}
