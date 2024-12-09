namespace Models.DTO
{
    public class AssignmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfParticipants { get; set; }

        public Guid StatusId { get; set; }
        public Guid PriorityId { get; set; }
    }
}
