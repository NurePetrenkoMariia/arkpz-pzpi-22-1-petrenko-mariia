namespace Models
{
    public class Farm
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }

        public User Owner { get; set; }
    }
}
