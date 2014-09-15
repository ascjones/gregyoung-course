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

        public Chef(IHandleOrder orderHandler)
        {
            _orderHandler = orderHandler;
        }

        private readonly IDictionary<string, string> ingredientDb = new Dictionary<string, string>()
        {
            {"Spaghetti Bolognese", "Pasta, Tomatoes, Mince"},
            {"Fish", "Cod, Chips, Mushy Peas"},
        };

        public void HandleOrder(Order order)
        {
            Thread.Sleep(1000);

            order.Ingredients = order.Items.Select(i => ingredientDb[i.ItemName]).ToArray();

            _orderHandler.HandleOrder(order);
        }
    }
}
