namespace DomainModels
{
    public class Medicine
    {
        public int ProductId { get; }
        public string Name { get; }
        public string Description { get; }
        public decimal Price { get; }

        public Medicine(int productId, string name, string description, decimal price)
        {
            ProductId = productId;
            Name = name;
            Description = description;
            Price = price;
        }
    }
}