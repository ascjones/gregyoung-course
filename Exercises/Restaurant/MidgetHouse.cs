using System;
using System.Collections.Generic;
using Restaurant.OrderHandlers;

namespace Restaurant
{
    public class MidgetHouse : IHandle<OrderPlaced>, IHandle<OrderCooked>, IHandle<OrderPriced>, IHandle<OrderPaid>
    {
        private readonly ITopicBasedPubSub bus;
        private readonly MidgetFactory midgedFactory;
        private readonly IDictionary<Guid, IMidget> midgets = new Dictionary<Guid, IMidget>();

        public MidgetHouse(ITopicBasedPubSub bus, MidgetFactory midgedFactory)
        {
            this.bus = bus;
            this.midgedFactory = midgedFactory;

            // todo: could wire this up using reflection
            bus.Subscribe<OrderPlaced>(this);
            bus.Subscribe<OrderCooked>(this);
            bus.Subscribe<OrderPriced>(this);
            bus.Subscribe<OrderPaid>(this);
        }

        public void AddMidget(Midget midget)
        {
            midgets.Add(midget.OrderId, midget);
        }

        public void Handle(OrderPlaced message)
        {
            var midget = midgedFactory.CreateMidget(bus, message.CorrelationId);
            midgets.Add(message.CorrelationId, midget);
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

        public void Handle(OrderPaid message)
        {
            midgets.Remove(message.CorrelationId);
        }
    }

}
