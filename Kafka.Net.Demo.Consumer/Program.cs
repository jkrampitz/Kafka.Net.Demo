using System;
using System.Threading;
using Confluent.Kafka;
using Kafka.Net.Demo.Infrastructure;
using Newtonsoft.Json;

namespace Kafka.Net.Demo.Consumer
{
    class Program
    {
        private static string _topic = "NetDemoTopic";
        private static EventTable _eventTable = new EventTable();
        static void Main(string[] args)
        {
            Console.WriteLine("Starting consumer...");
            ConsumerConfig config = CreateConsumerConfig();
            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };
            using (var consumer = new ConsumerBuilder<string, string>(config).Build())
            {
                consumer.Subscribe(_topic);

                try
                {
                    while (true)
                    {

                        var message = consumer.Consume(cts.Token);
                        if (message != null)
                        {
                            EventData eventData = JsonConvert.DeserializeObject<EventData>(message.Message.Value);
                            _eventTable.AddEvent(eventData);
                            Console.Clear();
                            var table = _eventTable.DisplayEventsAsStringTable();
                            foreach (var line in table)
                            {
                                Console.WriteLine(line);
                            }

                            Console.WriteLine();
                            Console.WriteLine(
                                $"Received message vor source: {message.Message.Key}, new message: {eventData.Message}.");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine($"Ctrl+C pressed, consumer exiting");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                finally
                {
                    consumer.Commit();
                    consumer.Close();
                }
            }
        }

        private static ConsumerConfig CreateConsumerConfig()
        {
            var config = new ClientConfig();
            config.BootstrapServers = "172.27.74.27";
            var consumerConfig = new ConsumerConfig(config);
            consumerConfig.GroupId = "NetDemoTopic_ConsumerGroup_1";
            consumerConfig.AutoOffsetReset = AutoOffsetReset.Earliest;
            consumerConfig.EnableAutoCommit = false;
            return consumerConfig;
        }
    }
}
