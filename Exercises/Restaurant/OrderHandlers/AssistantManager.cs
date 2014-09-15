using System.Collections.Generic;
using System.Linq;

namespace Restaurant.OrderHandlers
{
    public class AssistantManager : IHandleOrder
    {
        private readonly IHandleOrder orderHandler;

        private const decimal TaxRate = 0.2M;

        private Dictionary<string, decimal> dishPrices = new Dictionary<string, decimal>
        {
            { "Spaghetti Bolognese", 27.90M },
            { "Fish", 23.90M }
        }; 

        public AssistantManager(IHandleOrder orderHandler)
        {
            this.orderHandler = orderHandler;
        }

        public void HandleOrder(Order order)
        {
            var subtotal = order.Items.Select(item =>
            {
                var price = dishPrices[item.ItemName];
                return price * item.Qty;
            }).Sum();

            var tax = subtotal*TaxRate;

            order.Subtotal = subtotal;
            order.Tax = tax;
            order.Total = subtotal + tax;

            orderHandler.HandleOrder(order);
        }
    }
}
