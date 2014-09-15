using System;

namespace Restaurant
{
    public class Waiter
    {
        private readonly IHandleOrder _orderHandler;

        public Waiter(IHandleOrder orderHandler)
        {
            _orderHandler = orderHandler;
        }

        public string PlaceOrder()
        {
            var order = new Order
            {
                OrderId = "1",
                TableNumber = 5,
                ServerName = "Dave",
                TimeStamp = DateTime.Now.ToShortTimeString(),
                Items = new[]
                {
                    new OrderItem {ItemName = "Spaghetti Bolognese", Qty = 2},
                    new OrderItem {ItemName = "Fish", Qty = 3 },
                }
            };
            _orderHandler.HandleOrder(order);
            return order.OrderId;
        } 
    }
}
