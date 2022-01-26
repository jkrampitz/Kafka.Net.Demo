using System.Collections.Generic;
using Kafka.Net.Demo.Infrastructure;

namespace Kafka.Net.Demo.Consumer
{
    public class EventTable
    {
        private readonly Dictionary<string, EventData> _events;
        private string _lastEventSource;

        public EventTable()
        {
            _events = new Dictionary<string, EventData>();
        }

        public void AddEvent(EventData data)
        {
            _lastEventSource = data.Source;
            if (_events.ContainsKey(data.Source))
            {
                _events[data.Source] = data;
            }
            else
            {
                _events.Add(data.Source, data);
            }
        }

        public List<string> DisplayEventsAsStringTable()
        {
            var result = new List<string>();
            foreach (var eventData in _events)
            {
                var data = eventData.Value;
                var isLastChange = data.Source == _lastEventSource ? "*" : string.Empty;
                var line = string.Format("|{0} | {1} | {2} | {3}",
                    data.Source.PadRight(15, ' '),
                    data.CreatedOn.ToString().PadRight(20, ' '),
                    data.Message.PadRight(15, ' '),
                    isLastChange);
                result.Add(line);

            }
            return result;
        }
    }
}
