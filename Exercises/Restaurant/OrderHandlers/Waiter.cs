using System;
using System.Globalization;

namespace Restaurant
{
    public class Waiter
    {
        private readonly ITopicBasedPubSub bus;
        private int orderId = 1;

        public Waiter(ITopicBasedPubSub bus)
        {
            this.bus = bus;
        }

        public string PlaceOrder()
        {
            var order = new Order
            {
                OrderId = orderId++.ToString(CultureInfo.InvariantCulture),
                TableNumber = 5,
                ServerName = "Dave",
                TimeStamp = DateTime.Now.ToShortTimeString(),
                Items = new[]
                {
                    new OrderItem {ItemName = "Spaghetti Bolognese", Qty = 2},
                    new OrderItem {ItemName = "Fish", Qty = 3 },
                },
                LiveUntil = DateTime.UtcNow.Add(TimeSpan.FromSeconds(10.0))
            };
            bus.Publish(new OrderPlaced(order));
            return order.OrderId;
        } 
    }
}
