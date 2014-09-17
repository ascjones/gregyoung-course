using System;
using System.Collections.Generic;

namespace Restaurant.OrderHandlers
{
    public interface IMultiplexer
    {
    }

    public class Widener<TIn, TOut>
    {
        
    }

    public class Multiplexer<T> : IHandle<T>, IMultiplexer
    {
        private readonly IList<IHandle<T>> handlers;

        public Multiplexer(IEnumerable<IHandle<T>> handlers)
        {
            this.handlers = new List<IHandle<T>>(handlers);
        }

        public void Add(IHandle<T> handler)
        {
            handlers.Add(handler);
        }

        public void Handle(T msg)
        {
            foreach (var handler in handlers)
            {
                Console.WriteLine("Multiplexer delivering to {0}:{1}", handler.GetType().Name, typeof(T).Name);
                handler.Handle(msg);
            }
        }
    }
}
