using System;
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
            var startables = new List<IStartable>();
            var cashier = new Cashier(new ConsolePrintingOrderHandler());
            startables.Add(cashier);
            var assistantManager = new AssistantManager(cashier);

            var chefs = new List<ThreadedHandler>();

            for (int i = 0; i < NumberOfChefs; i++)
            {
                var chef = new Chef(assistantManager);
                var threaded = new ThreadedHandler(chef);
                chefs.Add(threaded);
                startables.Add(threaded);
            }

            //   var multiplexer = new Multiplexer(chefs);
            var roundRobin = new RoundRobin(chefs);

            foreach (var startable in startables)
            {
                startable.Start();
            }
            var monitor = new Monitor(startables);
            monitor.Start();

            var waiter = new Waiter(roundRobin);

            var orderId = waiter.PlaceOrder();

            Console.ReadKey();
        }
    }
}
