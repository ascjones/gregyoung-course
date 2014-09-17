using System;
using System.Collections.Generic;

namespace Restaurant.OrderHandlers
{
    public interface IMultiplexer
    {
    }

    public class Multiplexer : IHandler, IMultiplexer
    {
        //private readonly IList<IHandle<T>> handlers;
        //private readonly IList<Widener<TMessage, >> 
        private readonly IDictionary<Type, List<IHandler>> handlers = new Dictionary<Type, List<IHandler>>();

        //public Multiplexer(Dictionary<Type, IHandler> handlers)
        //{
        //    this.handlers = handlers;
        //}

        public void Add<T>(IHandle<T> handler)
        {
            List<IHandler> existingHandlers;
            if (!handlers.TryGetValue(typeof(T), out existingHandlers))
            {
                existingHandlers = new List<IHandler>();
                handlers.Add(typeof(T), existingHandlers);
            }
            existingHandlers.Add(handler);
        }

        public void Handle<T>(T msg)
        {
            List<IHandler> messageHandlers;
            if (handlers.TryGetValue(typeof(T), out messageHandlers))
            {
                foreach (var handler in messageHandlers)
                {
                    Console.WriteLine("Multiplexer delivering to {0}:{1}", handler.GetType().Name, typeof(T).Name);

                    var typedHandler = (IHandle<T>) handler;

                    typedHandler.Handle(msg);
                }
            }

        }
    }
}
