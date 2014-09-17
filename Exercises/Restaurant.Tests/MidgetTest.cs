using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Restaurant.OrderHandlers;

namespace Restaurant.Tests
{
    [TestFixture]
    public class MidgetTest
    {
      
        [Test]
        public void WhenMidgetNotifiedOfOrderPlaced_ThenMidgetSendsCommandToCookFood()
        {
            var orderGuid = Guid.NewGuid();
            var order = new Order
            {
                OrderId = "1",
                TableNumber = 5,
                ServerName = "Dave",
                TimeStamp = "12:00",
                TimeToCook = "00:00",
                Subtotal = 5.55M,
                Total = 6.66M,
                Tax = 1.11M,
                Ingredients = new[] {"Pasta", "Fish"},
                Paid = true,
                Items = new[]
                {
                    new OrderItem {ItemName = "5", Qty = 2, Price = 5.00M},
                    new OrderItem {ItemName = "6", Qty = 3, Price = 6.00M},
                }
            };

            var orderPlaced = new OrderPlaced(order, Guid.NewGuid(), orderGuid);

            var bus = new FakeBus();


            var midget = new Midget(bus, orderGuid);
            midget.Handle(orderPlaced);

            //var cookFood = (CookFood) bus.Messages.Single();
            Assert.That(bus.Messages.Single(), Is.InstanceOf<CookFood>());
        }
    }
}
