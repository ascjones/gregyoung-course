﻿using System;
using System.Globalization;

namespace Restaurant.OrderHandlers
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
            DateTime liveUntil = DateTime.UtcNow.Add(TimeSpan.FromSeconds(10.0));
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
                LiveUntil = liveUntil
            };
            bus.Publish(new CookFood(order, Guid.Empty, Guid.NewGuid(), liveUntil));
            return order.OrderId;
        } 
    }
}
