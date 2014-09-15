using Restaurant;
using Restaurant.OrderHandlers;

namespace Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            var cashier = new Cashier(new ConsolePrintingOrderHandler());
            var assistantManager = new AssistantManager(cashier);
            var chef = new Chef(assistantManager);

            var waiter = new Waiter(chef);
            var orderId = waiter.PlaceOrder();

            cashier.Pay(orderId);

        }
    }
}
