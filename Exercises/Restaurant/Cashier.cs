using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
