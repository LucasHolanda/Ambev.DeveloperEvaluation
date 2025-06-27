namespace Ambev.DeveloperEvaluation.Application.Publisher
{
    public class EventMessage<T>
    {
        public string EventName { get; set; } = string.Empty;
        public Guid EventId { get; set; }
        public DateTime Timestamp { get; set; }
        public T Data { get; set; } = default!;
        public string Source { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
    }

    public class MessagePublishException : Exception
    {
        public MessagePublishException(string message) : base(message) { }
        public MessagePublishException(string message, Exception innerException) : base(message, innerException) { }
    }
}
