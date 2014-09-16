using System.Collections.Generic;

namespace Restaurant.OrderHandlers
{
    public class RoundRobin : IHandleOrder
    {
        private readonly Queue<IHandleOrder> handlers;

        public RoundRobin(IEnumerable<IHandleOrder> handlers)
        {
            this.handlers = new Queue<IHandleOrder>(handlers);
        }

        public void HandleOrder(Order order)
        {
            var handler = handlers.Dequeue();

            try
            {
                handler.HandleOrder(order);
            }
            finally 
            {
                handlers.Enqueue(handler);
            }
        }
    }
}
