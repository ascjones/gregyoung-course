using System;
using Restaurant.OrderHandlers;

namespace Restaurant
{

    public interface IMidget : IHandle<OrderPlaced>, IHandle<OrderCooked>, IHandle<OrderPriced>
    {
    }

    public class Midget : IMidget
    {
        private readonly IMessagePublisher bus;
        private readonly Guid orderId;

        public Midget(IMessagePublisher bus, Guid orderId)
        {
            this.bus = bus;
            this.orderId = orderId;
        }

        public Guid OrderId
        {
            get { return orderId; }
        }

        public void Handle(OrderPlaced message)
        {
            bus.Publish(new CookFood(message.Order, message.MessageId, message.CorrelationId, DateTime.UtcNow.AddSeconds(10)));
        }

        public void Handle(OrderCooked message)
        {
            bus.Publish(new PriceOrder(message.Order, message.MessageId, message.CorrelationId));
        }

        public void Handle(OrderPriced message)
        {
            bus.Publish(new TakePayment(message.Order, message.MessageId, message.CorrelationId));
        }
    }
}