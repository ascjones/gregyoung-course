using System.Collections.Generic;

namespace Restaurant.OrderHandlers
{
    public class Multiplexer : IHandleOrder
    {
        private IEnumerable<IHandleOrder> orderHandlers;

        public Multiplexer(IEnumerable<IHandleOrder> orderHandlers)
        {
            this.orderHandlers = orderHandlers;
        }

        public void HandleOrder(Order order)
        {
            foreach (var handler in orderHandlers)
            {
                handler.HandleOrder(order);
            }
        }
    }
}
