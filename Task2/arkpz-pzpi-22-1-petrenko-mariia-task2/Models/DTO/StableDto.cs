namespace Models.DTO
{
    public class StableDto
    {
        public Guid Id { get; set; }
        public int MinFeedLevel { get; set; }
        public int CurrentFeedLevel { get; set; }
        public DateTime DateTimeOfUpdate { get; set; }

        public Guid FarmId { get; set; }
    }
}
