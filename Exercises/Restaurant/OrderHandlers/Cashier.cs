using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.OrderHandlers
{
    public class Cashier : IHandle<OrderPriced>, IStartable
    {
        private readonly ITopicBasedPubSub bus;
        private readonly ConcurrentQueue<Order> orders = new ConcurrentQueue<Order>();

        public Cashier(ITopicBasedPubSub bus)
        {
            this.bus = bus;
        }

        public void Handle(OrderPriced message)
        {
            orders.Enqueue(message.Order);
        }

        public void HandleOutstandingPayments()
        {
            while (true)
            {
                Order order;
                while (orders.TryDequeue(out order))
                {
                    order.Paid = true;
                    bus.Publish(new OrderPaid(order));
                }
                Thread.Sleep(1);
            }       
        }

        public void Start()
        {
            Task.Factory.StartNew(HandleOutstandingPayments, TaskCreationOptions.LongRunning);
        }

        public string GetStatistics()
        {
            return string.Format("Cashier queue count {0}", orders.Count);
        }

        
    }
}
