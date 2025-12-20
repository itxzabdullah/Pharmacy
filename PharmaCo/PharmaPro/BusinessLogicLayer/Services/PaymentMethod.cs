using System;

namespace BusinessLogicLayer.Services
{
    public abstract class PaymentMethod
    {
        public abstract void Pay(decimal amount);
    }

    public class CashOnDelivery : PaymentMethod
    {
        public override void Pay(decimal amount)
        {
            Console.WriteLine($"Cash on Delivery selected. Amount: {amount}");
        }
    }

    public class OnlinePayment : PaymentMethod
    {
        public override void Pay(decimal amount)
        {
            Console.WriteLine($"Online Payment processed. Amount: {amount}");
        }
    }
}