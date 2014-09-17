using System;
using System.Collections.Generic;
using Restaurant.OrderHandlers;

namespace Restaurant
{
    public class MidgetHouse : IHandle<OrderPlaced>, IHandle<OrderCooked>, IHandle<OrderPriced>
    {
        private readonly ITopicBasedPubSub bus;
        private IDictionary<Guid, Midget> midgets = new Dictionary<Guid, Midget>();

        public MidgetHouse(ITopicBasedPubSub bus)
        {
            this.bus = bus;

            // todo: could wire this up using reflection
            bus.Subscribe<OrderPlaced>(this);
            bus.Subscribe<OrderCooked>(this);
            bus.Subscribe<OrderPriced>(this);
        }

        public void AddMidget(Midget midget)
        {
            midgets.Add(midget.OrderId, midget);
        }

        public void Handle(OrderPlaced message)
        {
            midgets[message.CorrelationId].Handle(message);
        }

        public void Handle(OrderCooked message)
        {
            midgets[message.CorrelationId].Handle(message);
        }

        public void Handle(OrderPriced message)
        {
            midgets[message.CorrelationId].Handle(message);
        }
    }

    public class MidgetFactory
    {

        public IMidget CreateMidget(ITopicBasedPubSub bus, Guid orderId)
        {
            return new Midget(bus, orderId);
        }
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

    public interface IMidget
    {
        void Handle(OrderPlaced message);
    }
}
