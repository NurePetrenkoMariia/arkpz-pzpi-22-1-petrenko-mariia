namespace Models
{
    public class UsersTask
    {
        public Guid Id { get; set; }

        public User User { get; set; }
        public Task Task { get; set; }

    }
}
