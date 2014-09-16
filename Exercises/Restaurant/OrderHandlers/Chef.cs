using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant
{
    public class Chef : IHandleOrder
    {
        private readonly ITopicBasedPubSub bus;
        private readonly int timeToCook;

        public Chef(ITopicBasedPubSub bus, int timeToCook)
        {
            this.bus = bus;
            this.timeToCook = timeToCook;
        }

        private readonly IDictionary<string, string> ingredientDb = new Dictionary<string, string>()
        {
            {"Spaghetti Bolognese", "Pasta, Tomatoes, Mince"},
            {"Fish", "Cod, Chips, Mushy Peas"},
        };

        public void HandleOrder(Order order)
        {
            Thread.Sleep(timeToCook);

            order.Ingredients = order.Items.Select(i => ingredientDb[i.ItemName]).ToArray();

            bus.Publish(Messages.OrderPrepared, order);

        }
    }
}
