using System;

namespace Restaurant
{
    public interface IMessage
    {
        Guid MessageId { get; }
        DateTime? TimeToLive { get; }
    }

    public class BaseEvent : IMessage
    {
        public Guid MessageId { get; set; }
        public DateTime? TimeToLive { get; set; }
    }

    public class OrderPlaced : BaseEvent
    {
        public OrderPlaced(Order order)
        {
            Order = order;
        }
        public Order Order { get; private set; }
    }

    public class OrderCooked : BaseEvent
    {
        public OrderCooked(Order order)
        {
            Order = order;
        }
        public Order Order { get; private set; }
    }

    public class OrderPriced : BaseEvent
    {
        public OrderPriced(Order order)
        {
            Order = order;
        }

        public Order Order { get; private set; }
    }

    public class OrderPaid : BaseEvent
    {
        public OrderPaid(Order order)
        {
            Order = order;
        }

        public Order Order { get; private set; }
    }

}
