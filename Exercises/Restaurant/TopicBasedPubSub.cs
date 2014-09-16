using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Restaurant.OrderHandlers;

namespace Restaurant
{
    public class TopicBasedPubSub : ITopicBasedPubSub
    {
        private readonly IDictionary<string, Multiplexer> subscriptions = new ConcurrentDictionary<string, Multiplexer>(); 

        public void Subscribe(string topic, IHandleOrder handler)
        {
            Multiplexer multiplexer;
            if (!subscriptions.TryGetValue(topic, out multiplexer))
            {
                multiplexer = new Multiplexer(Enumerable.Empty<IHandleOrder>());
                subscriptions.Add(topic, multiplexer);
            }
            multiplexer.Add(handler);
        }

        public void Publish(string topic, Order order)
        {
    //        Console.WriteLine("Publishing message {0}", topic);
            Multiplexer multiplexer;
            if (subscriptions.TryGetValue(topic, out multiplexer))
            {
                multiplexer.HandleOrder(order);
            }
        }
    }

    public interface ITopicBasedPubSub
    {
        void Subscribe(string topic, IHandleOrder handler);
        void Publish(string topic, Order order);
    }
}
