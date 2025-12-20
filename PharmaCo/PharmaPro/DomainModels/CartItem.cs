using System;

namespace DomainModels
{
    public class CartItem
    {
        public decimal UnitPrice { get; private set; }
        public int ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }

        public CartItem(int productId, string productName, decimal unitPrice, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be positive.");

            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }

        public decimal TotalPrice
        {
            get { return UnitPrice * Quantity; }
        }

        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
                throw new ArgumentException("Quantity must be positive.");
            Quantity = newQuantity;
        }
    }
}