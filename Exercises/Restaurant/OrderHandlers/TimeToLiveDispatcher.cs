using System;

namespace Restaurant.OrderHandlers
{
    public class TimeToLiveDispatcher : IHandleOrder
    {
        private readonly IHandleOrder orderHandler;

        public TimeToLiveDispatcher(IHandleOrder orderHandler)
        {
            this.orderHandler = orderHandler;
        }

        public void HandleOrder(Order order)
        {
            if (order.LiveUntil > DateTime.UtcNow)
            {
                orderHandler.HandleOrder(order);
            }
            else
            {
                Console.WriteLine("Message past TTL, discarding. {0}", order.OrderId);
            }
        }
    }
}
