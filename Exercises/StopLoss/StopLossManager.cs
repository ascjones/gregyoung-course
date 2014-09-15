using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StopLoss
{
    public class StopLossManager
    {
        private readonly IMessageBus _bus;
        private readonly List<decimal> priceUpdates = new List<decimal>();

        private decimal initialPosition;
        private decimal currentStopTarget;

        public StopLossManager(IMessageBus bus)
        {
            _bus = bus;
        }

        public void Consume(PositionAcquired positionAcquired)
        {
            initialPosition = positionAcquired.Price;  
            RaiseTargetUpdated(positionAcquired.Price);
        }

        public void Consume(PriceUpdated priceUpdated)
        {
            priceUpdates.Add(priceUpdated.Price);
            
            _bus.Publish(new SendToMeInX<PriceUpdated>(10.0M, priceUpdated));
            _bus.Publish(new SendToMeInX<PriceUpdated>(7.0M, priceUpdated));
        }

        public void Consume(SendToMeInX<PriceUpdated> msg)
        {
            var minPrice = priceUpdates.Any() ? priceUpdates.Min() : initialPosition;
            if (minPrice > currentStopTarget)
            {
                RaiseTargetUpdated(minPrice);   
            }

            var maxPrice = priceUpdates.Any() ? priceUpdates.Max() : initialPosition;
            if (maxPrice < currentStopTarget)
            {
                _bus.Publish(new StopLossTriggered());
            }
        }

        private void RaiseTargetUpdated(decimal price)
        {
            currentStopTarget = price - StopLossThreshold;
            var targetUpdated = new TargetUpdated(currentStopTarget);
            _bus.Publish(targetUpdated); 
        }

        public const decimal StopLossThreshold = 0.1m;
    }

    public interface IMessageBus
    {
        void Publish(IMessage msg);
    }
}