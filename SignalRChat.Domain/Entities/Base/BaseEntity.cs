namespace SignalRChat.Domain.Entities.Base
{
    public abstract class BaseEntity 
    {
        public Guid Id { get; } = Guid.NewGuid();
        public DateTime CreatedDate { get; } = DateTime.Now;
    }
}
