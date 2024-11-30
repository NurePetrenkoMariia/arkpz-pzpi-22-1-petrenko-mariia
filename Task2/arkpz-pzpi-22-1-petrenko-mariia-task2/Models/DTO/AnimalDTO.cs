namespace FarmKeeper.Models.DTO
{
    public class AnimalDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Sex { get; set; }

        public Guid FarmId { get; set; }
        public Farm Farm { get; set; }
        public Guid StableId { get; set; }
        public Stable Stable { get; set; }
    }
}
