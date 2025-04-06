namespace EventBus.Base.Events
{
    public class IntegrationEvent
    {
        public Guid Id { get; }
        public DateTime CreatedDate { get; }

        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.Now;
        }

        public IntegrationEvent(Guid id, DateTime createDate)
        {
            Id = id;
            CreatedDate = createDate;
        }
    }
}
