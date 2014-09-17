using System;

namespace Restaurant
{
    public class MidgetFactory
    {

        public IMidget CreateMidget(ITopicBasedPubSub bus, Guid orderId)
        {
            return new Midget(bus, orderId);
        }
    }
}