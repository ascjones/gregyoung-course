using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant
{
    public class Cashier : IHandleOrder
    {
        private readonly IHandleOrder handler;
        private readonly ConcurrentQueue<Order> orders = new ConcurrentQueue<Order>();

        public Cashier(IHandleOrder handler)
        {
            this.handler = handler;
        }

        public void HandleOrder(Order order)
        {
            orders.Enqueue(order);
        }

        public void HandleOutstandingPayments()
        {
            while (true)
            {
                Order order;
                while (orders.TryDequeue(out order))
                {
                    order.Paid = true;
                    handler.HandleOrder(order);
                }
                Thread.Sleep(1);
            }       
        }

        public void Start()
        {
            Task.Factory.StartNew(HandleOutstandingPayments, TaskCreationOptions.LongRunning);
        }
    }
}
