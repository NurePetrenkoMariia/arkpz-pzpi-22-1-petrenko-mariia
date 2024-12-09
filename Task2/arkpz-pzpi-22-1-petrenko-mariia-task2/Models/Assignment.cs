namespace Models
{
    public class Assignment
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfParticipants { get; set; }

        public Guid StatusId { get; set; }
        public Status Status { get; set; }
        public Guid PriorityId { get; set; }
        public Priority Priority { get; set; }
        
    }
}
