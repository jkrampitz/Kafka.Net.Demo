using System;

namespace Kafka.Net.Demo.Infrastructure
{
    public class EventData
    {
        public Guid Id { get; set; }
        public string Source { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Message { get; set; }
    }
}
