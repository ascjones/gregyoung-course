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
        private readonly IHandleOrder _orderHandler;
        private readonly int timeToCook;

        public Chef(IHandleOrder orderHandler, int timeToCook)
        {
            _orderHandler = orderHandler;
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

            _orderHandler.HandleOrder(order);
        }
    }
}
