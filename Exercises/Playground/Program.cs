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
            var rand = new Random();
            for (int i = 0; i < NumberOfChefs; i++)
            {
                var chef = new Chef(assistantManager, rand.Next(1000));
                var threaded = new ThreadedHandler(string.Format("Chef {0}", i) ,chef);
                chefs.Add(threaded);
                startables.Add(threaded);
            }

            var distributionStrategy = new QueuedHandler(chefs);
            startables.Add(distributionStrategy);

            foreach (var startable in startables)
            {
                startable.Start();
            }
            var monitor = new Monitor(startables);
            monitor.Start();

            var waiter = new Waiter(distributionStrategy);

            for (int i = 0; i < 1000; i++)
            {
                var orderId = waiter.PlaceOrder();    
            }            

            Console.ReadKey();
        }
    }
}
