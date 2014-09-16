using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Restaurant.OrderHandlers;

namespace Restaurant
{
    public class TopicBasedPubSub : ITopicBasedPubSub
    {
        private readonly IDictionary<string, IMultiplexer> subscriptions = new ConcurrentDictionary<string, IMultiplexer>(); 

        public void Subscribe<T>(IHandle<T> handler, string topic = null)
        {
            Console.WriteLine("Subscribing to {0}", typeof(T).Name);
            IMultiplexer multiplexer;

            topic = topic ?? typeof (T).Name;
            if (!subscriptions.TryGetValue(topic, out multiplexer))
            {
                multiplexer = new Multiplexer<T>(Enumerable.Empty<IHandle<T>>());
                subscriptions.Add(topic, multiplexer);
            }
            ((Multiplexer<T>) multiplexer).Add(handler);
        }

        public void Publish<T>(T message, string topic = null)
        {
            Console.WriteLine("Publishing to {0}", typeof(T).Name);
            topic = topic ?? typeof(T).Name;
            IMultiplexer multiplexer;
            if (subscriptions.TryGetValue(topic, out multiplexer))
            {
                var typedMultiplexer = (Multiplexer<T>)  multiplexer;
                typedMultiplexer.Handle(message);
            }
        }
    }

    public interface ITopicBasedPubSub
    {
        void Subscribe<T>(IHandle<T> handler, string topic = null);
        void Publish<T>(T message, string topic = null);
    }
}
