using System;
using System.Collections.Generic;
using System.Threading;
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

            var chefs = new List<ThreadedHandler>();

            for (int i = 0; i < NumberOfChefs; i++)
            {
                var chef = new Chef(assistantManager);
                var threaded = new ThreadedHandler(chef);
                chefs.Add(threaded);
            }

            //   var multiplexer = new Multiplexer(chefs);
            var roundRobin = new RoundRobin(chefs);

            foreach (var chef in chefs)
            {
                chef.Start();
            }

            var waiter = new Waiter(roundRobin);

            var orderId = waiter.PlaceOrder();

            cashier.Start();

            Console.ReadKey();
        }
    }

}
