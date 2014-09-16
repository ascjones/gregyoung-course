using System;
using System.Collections.Generic;

namespace Restaurant.OrderHandlers
{
    public class Multiplexer : IHandleOrder
    {
        private readonly IList<IHandleOrder> orderHandlers;

        public Multiplexer(IEnumerable<IHandleOrder> orderHandlers)
        {
            this.orderHandlers = new List<IHandleOrder>(orderHandlers);
        }

        public void Add(IHandleOrder handler)
        {
            orderHandlers.Add(handler);
        }

        public void HandleOrder(Order order)
        {
            foreach (var handler in orderHandlers)
            {
            //    Console.WriteLine("Multiplexer delivering to {0}", handler.GetType().Name);
                handler.HandleOrder(order);
            }
        }
    }
}
