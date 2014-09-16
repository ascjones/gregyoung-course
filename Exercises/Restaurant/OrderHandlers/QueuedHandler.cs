using System.Collections.Concurrent;
using System.Threading;

namespace Restaurant.OrderHandlers
{
    public class QueuedHandler : IHandleOrder, IStartable
    {
        private readonly ConcurrentQueue<Order> workQueue = new ConcurrentQueue<Order>();
        private readonly IHandleOrder orderHandler;
        private readonly Thread workerThread;

        public QueuedHandler(string name, IHandleOrder orderHandler)
        {
            this.orderHandler = orderHandler;

            workerThread = new Thread(OrderHandler) {Name = name};
        }

        public decimal QueueCount { get { return workQueue.Count;  } }

        private void OrderHandler()
        {
            while (true)
            {
                Order order;
                if (workQueue.TryDequeue(out order))
                    orderHandler.HandleOrder(order);
                else
                    Thread.Sleep(1);
            }
        }

        public void HandleOrder(Order order)
        {
            workQueue.Enqueue(order);
        }

        public void Start()
        {
            workerThread.Start();
        }

        public string GetStatistics()
        {
            return string.Format("{0} queue count {1}", workerThread.Name, workQueue.Count);
        }
    }
}
