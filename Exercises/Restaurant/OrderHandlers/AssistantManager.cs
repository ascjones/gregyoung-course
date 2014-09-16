using System.Collections.Generic;
using System.Linq;

namespace Restaurant.OrderHandlers
{
    public class AssistantManager : IHandleOrder
    {
        private readonly ITopicBasedPubSub bus;
        private const decimal TaxRate = 0.2M;

        private readonly Dictionary<string, decimal> dishPrices = new Dictionary<string, decimal>
        {
            { "Spaghetti Bolognese", 27.90M },
            { "Fish", 23.90M }
        }; 

        public AssistantManager(ITopicBasedPubSub bus)
        {
            this.bus = bus;
            bus.Subscribe(Messages.OrderPrepared, this);
        }

        public void HandleOrder(Order order)
        {
            decimal subtotal = 0M;
            foreach (var item in order.Items)
            {
                var price = dishPrices[item.ItemName];
                item.Price = price;
                subtotal += price * item.Qty;
            }

            var tax = subtotal*TaxRate;

            order.Subtotal = subtotal;
            order.Tax = tax;
            order.Total = subtotal + tax;

            bus.Publish(Messages.OrderBilled, order);
        }
    }
}
