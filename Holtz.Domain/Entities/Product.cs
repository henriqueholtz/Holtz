namespace Holtz.Domain.Entities
{
    public class Product
    {
        public Product(string name, string description, double price )
        {
            this.Id = Guid.NewGuid();
            this.Description = description;
            this.Name = name;
            this.Price = price;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
    }
}
