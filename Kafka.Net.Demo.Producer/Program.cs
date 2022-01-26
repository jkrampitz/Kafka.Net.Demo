using System;
using System.Threading;
using Confluent.Kafka;
using Kafka.Net.Demo.Infrastructure;
using Newtonsoft.Json;

namespace Kafka.Net.Demo.Producer
{
    class Program
    {
        private static string _topic = "NetDemoTopic";
        private static Random Rnd = new Random();
        private static bool _isCancelled = false;
        static void Main(string[] args)
        {
            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;
                _isCancelled = true;
                Console.WriteLine("Stopped by user");
            };
            Console.WriteLine("Starting Producer for Kafka-Topic " + _topic + "...");
            IProducer<string, string> producer = null;
            int i = 0;
            try
            {
                
                producer = new ProducerBuilder<string, string>(CreateProducerConfig()).Build();
                while (_isCancelled == false)
                {
                    i++;
                    var message = $"message_{i}";
                    var eventdata = EventGenerator.GenerateRandomEvent(message);
                    producer.Produce(_topic, new Message<string, string> {Key = eventdata.Source, Value = JsonConvert.SerializeObject(eventdata, Formatting.Indented)},
                        (deliveryReport) =>
                        {
                            if (deliveryReport.Error.Code != ErrorCode.NoError)
                            {
                                Console.WriteLine($"Failed to deliver message: {deliveryReport.Error.Reason}");
                            }
                            else
                            {
                                Console.WriteLine($"Produced Event (Source: {eventdata.Source}, Message: {eventdata.Message}) to: {deliveryReport.TopicPartitionOffset}");
                            }
                        });
                    producer.Flush(TimeSpan.FromSeconds(5));
                    Thread.Sleep(Rnd.Next(5000));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                if (producer != null)
                {
                    var queueSize = producer.Flush(TimeSpan.FromSeconds(5));
                    if (queueSize > 0)
                    {
                        Console.WriteLine("WARNING: Producer event queue has " + queueSize + " pending events on exit.");
                    }
                    producer.Dispose();
                }
            }
        }

        public static ClientConfig CreateProducerConfig()
        {
            var config = new ClientConfig();
            config.BootstrapServers = "172.27.74.27";
            return config;
        }
    }
}
