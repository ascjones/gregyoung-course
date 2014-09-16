using System;
using System.Collections.Generic;
using Restaurant;
using Restaurant.OrderHandlers;

namespace Playground
{
    class Program
    {
        private const int NumberOfChefs = 3;

        static void Main()
        {
            var bus = new TopicBasedPubSub();
            var startables = new List<IStartable>();
            var consolePrinter = new QueuedHandler<OrderPaid>(bus, Messages.Paid, new ConsolePrintingOrderHandler(bus));
            startables.Add(consolePrinter);
            var cashier = new Cashier(bus);
            var queuedCashier = new QueuedHandler<OrderPriced>(bus, Messages.OrderBilled, cashier);
            startables.Add(queuedCashier);
            startables.Add(cashier);

            var assistantManager = new QueuedHandler<OrderCooked>(bus, Messages.OrderPrepared, new AssistantManager(bus));
            startables.Add(assistantManager);
          
            var chefs = new List<QueuedHandler<OrderPlaced>>();
            var rand = new Random();
            for (int i = 0; i < NumberOfChefs; i++)
            {
                var chef = new TimeToLiveDispatcher<OrderPlaced>(new Chef(bus, rand.Next(1000)));
                var queuedHandler = new QueuedHandler<OrderPlaced>(bus, string.Format("Chef {0}", i), chef, true);
                chefs.Add(queuedHandler);
                startables.Add(queuedHandler);
            }

            var distributionStrategy = new QueuedDispatcher<OrderPlaced>(bus, chefs);
            startables.Add(distributionStrategy);

            foreach (var startable in startables)
            {
                startable.Start();
            }
            var monitor = new Monitor(startables);
            monitor.Start();

            var waiter = new Waiter(bus);

            for (int i = 0; i < 1; i++)
            {
                var orderId = waiter.PlaceOrder();    
            }            

            Console.ReadKey();
        }
    }
}
