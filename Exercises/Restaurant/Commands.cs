using System;

namespace Restaurant
{
    class Commands
    {
    }

    public class CookFood : IMessage
    {
        public CookFood(Order order)
        {
            Order = order;
        }

        public Guid MessageId { get; private set; }
        public DateTime? TimeToLive { get; private set; }

        public Order Order { get; set; }
    }

    public class PriceOrder : IMessage
    {
        public PriceOrder(Order order)
        {
            Order = order;
        }

        public Guid MessageId { get; private set; }
        public DateTime? TimeToLive { get; private set; }

        public Order Order { get; set; }
    }

    public class TakePayment : IMessage
    {
        public TakePayment(Order order)
        {
            Order = order;
        }

        public Guid MessageId { get; private set; }
        public DateTime? TimeToLive { get; private set; }

        public Order Order { get; set; }
    }
}
