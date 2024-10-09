namespace BookProject.Events
{
    public abstract class Event
    {
        public abstract Guid Id { get; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
