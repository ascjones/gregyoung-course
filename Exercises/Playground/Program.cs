using System.Collections.Generic;
using Restaurant;
using Restaurant.OrderHandlers;

namespace Playground
{
    class Program
    {
        private const int NumberOfChefs = 3;

        static void Main(string[] args)
        {
            var cashier = new Cashier(new ConsolePrintingOrderHandler());
            var assistantManager = new AssistantManager(cashier);

            var chefs = new List<Chef>();

            for (int i = 0; i < NumberOfChefs; i++)
            {
                chefs.Add(new Chef(assistantManager));
            }

            var multiplexer = new Multiplexer(chefs);

            var waiter = new Waiter(multiplexer);
            var orderId = waiter.PlaceOrder();

            cashier.Pay(orderId);

        }
    }
}
