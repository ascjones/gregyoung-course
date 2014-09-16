using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.OrderHandlers
{
    public class QueuedHandler : IStartable, IHandleOrder
    {
        private readonly ConcurrentQueue<Order>  outerQueue = new ConcurrentQueue<Order>();
        private readonly IEnumerable<ThreadedHandler> childHandlers;

        public QueuedHandler(IEnumerable<ThreadedHandler> childHandlers)
        {
            this.childHandlers = childHandlers;
        }

        public void Start()
        {
            Task.Factory.StartNew(StartProcessingOrders, TaskCreationOptions.LongRunning);
        }

        private void StartProcessingOrders()
        {
            while (true)
            {
                Order order;
                while (outerQueue.TryDequeue(out order))
                {
                    bool orderPending = true;
                    while (orderPending)
                    {
                        foreach (var childHandler in childHandlers)
                        {
                            if (childHandler.QueueCount < 5)
                            {
                                try
                                {
                                    childHandler.HandleOrder(order);
                                    orderPending = false;
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    // todo: deal wioth poison messages?
                                    Console.WriteLine("Error handling order: {0}", ex);
                                }
                           }
                        }
                        Thread.Sleep(1);
                    }
                }
                Thread.Sleep(1);
            }
        }

        public string GetStatistics()
        {
            return string.Format("Outer queue length: {0}", outerQueue.Count);
        }

        public void HandleOrder(Order order)
        {
            outerQueue.Enqueue(order);
        }
    }
}
