using System;
using Restaurant;

namespace Playground
{
    class ConsolePrintingOrderHandler : IHandleOrder
    {
        public void HandleOrder(Order order)
        {
            //Console.WriteLine(order.Serialize().ToString());
        }
    }
}
