using System;
using Restaurant;

namespace Playground
{
    class ConsolePrintingOrderHandler : IHandleOrder
    {
        private readonly ITopicBasedPubSub bus;

        public ConsolePrintingOrderHandler(ITopicBasedPubSub bus)
        {
            this.bus = bus;
        }

        public void HandleOrder(Order order)
        {
        //    Console.WriteLine(order.Serialize().ToString());
        }
    }
}
