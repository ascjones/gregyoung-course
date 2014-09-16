using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Restaurant.OrderHandlers;

namespace Restaurant
{
    public class TopicBasedPubSub : ITopicBasedPubSub
    {
        private readonly IDictionary<Type, IMultiplexer> subscriptions = new ConcurrentDictionary<Type, IMultiplexer>(); 

        public void Subscribe<T>(IHandle<T> handler)
        {
            Console.WriteLine("Subscribing to {0}", typeof(T).Name);
            IMultiplexer multiplexer;
            var topic = typeof (T);
            if (!subscriptions.TryGetValue(topic, out multiplexer))
            {
                multiplexer = new Multiplexer<T>(Enumerable.Empty<IHandle<T>>());
                subscriptions.Add(topic, multiplexer);
            }
            ((Multiplexer<T>) multiplexer).Add(handler);
        }

        public void Publish<T>(T message)
        {
            Console.WriteLine("Publishing to {0}", typeof(T).Name);
            IMultiplexer multiplexer;
            if (subscriptions.TryGetValue(typeof (T), out multiplexer))
            {
                var typedMultiplexer = (Multiplexer<T>)  multiplexer;
                typedMultiplexer.Handle(message);
            }
        }
    }

    public interface ITopicBasedPubSub
    {
        void Subscribe<T>(IHandle<T> handler);
        void Publish<T>(T message);
    }
}
