using System.Collections.Generic;

namespace Restaurant
{
    public class Cashier : IHandleOrder
    {
        private readonly IHandleOrder _handler;
        private IDictionary<string, Order> orders = new Dictionary<string, Order>();

        public Cashier(IHandleOrder handler)
        {
            _handler = handler;
        }

        public void HandleOrder(Order order)
        {
            if (!orders.ContainsKey(order.OrderId))
                orders.Add(order.OrderId, order);
            _handler.HandleOrder(order);
        }

        public void Pay(string orderId)
        {
            orders[orderId].Paid = true;
            _handler.HandleOrder(orders[orderId]);
        }
    }
}
