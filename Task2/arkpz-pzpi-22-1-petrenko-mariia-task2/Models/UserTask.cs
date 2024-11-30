namespace FarmKeeper.Models
{
    public class UserTask
    {
        public Guid Id { get; set; }

        public User User { get; set; }
        public Assignment Assignment { get; set; }

    }
}
