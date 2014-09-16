using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Restaurant.OrderHandlers;

namespace Restaurant
{
    public class Chef : IHandle<OrderPlaced>
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

        public void Handle(OrderPlaced message)
        {
            Thread.Sleep(timeToCook);
            var order = message.Order;
            order.Ingredients = order.Items.Select(i => ingredientDb[i.ItemName]).ToArray();

            bus.Publish(new OrderCooked(order));
        }
    }
}
