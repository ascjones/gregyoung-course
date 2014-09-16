using System.Collections.Concurrent;
using System.Threading;

namespace Restaurant.OrderHandlers
{
    public interface IStartable
    {
        void Start();
    }

    public class ThreadedHandler : IHandleOrder, IStartable
    {
        private readonly ConcurrentQueue<Order> workQueue = new ConcurrentQueue<Order>();
        private readonly IHandleOrder orderHandler;
        private readonly Thread workerThread;

        public ThreadedHandler(IHandleOrder orderHandler)
        {
            this.orderHandler = orderHandler;

            workerThread = new Thread(OrderHandler);
        }

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
    }
}
