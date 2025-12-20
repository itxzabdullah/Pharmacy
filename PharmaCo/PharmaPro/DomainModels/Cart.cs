using System.Collections.Generic;
using System.Linq;

namespace DomainModels
{
    public class Cart
    {
        public List<CartItem> Items { get; private set; }

        public Cart()
        {
            Items = new List<CartItem>();
        }

        // Add item (if product already exists, update quantity)
        public void AddItem(CartItem item)
        {
            var existingItem = Items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.UpdateQuantity(existingItem.Quantity + item.Quantity);
            }
            else
            {
                Items.Add(item);
            }
        }

        // Remove item by ProductId
        public void RemoveItem(int productId)
        {
            Items.RemoveAll(i => i.ProductId == productId);
        }

        // Clear entire cart
        public void ClearCart()
        {
            Items.Clear();
        }

        // Calculate total cart value
        public decimal GetCartTotal()
        {
            return Items.Sum(i => i.TotalPrice);
        }
    }
}