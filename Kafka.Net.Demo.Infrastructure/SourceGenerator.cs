using System;
using System.Collections.Generic;
using System.Text;

namespace Kafka.Net.Demo.Infrastructure
{
    public static class SourceGenerator
    {
        private static readonly Random Rnd = new Random();
        private static readonly string[] Sources = {"TestDb", "MessageQueue", "Network", "App_1", "App_2", "RestApi", "Security", "Firewall", "LoadBalancer"};

        public static string GetRandomSource()
        {
            var index = Rnd.Next(Sources.Length);
            return Sources[index];
        }
    }
}
