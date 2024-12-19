namespace FarmKeeper.Models
{
    public class Stable
    {
        public Guid Id { get; set; }
        public int MinFeedLevel { get; set; }
        public int CurrentFeedLevel { get; set; }
        public DateTime DateTimeOfUpdate { get; set; }

        public Guid FarmId { get; set; }
        public Farm Farm { get; set; }
        public ICollection<Animal> Animals { get; set; }
    }
}
