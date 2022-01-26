using System;

namespace Kafka.Net.Demo.Infrastructure
{
    public static class EventGenerator
    {
        public static EventData GenerateRandomEvent(string message)
        {
            var data = new EventData();
            data.Id = Guid.NewGuid();
            data.CreatedOn = DateTime.Now;
            data.Source = SourceGenerator.GetRandomSource();
            data.Message = message;
            return data;
        }
    }
}
