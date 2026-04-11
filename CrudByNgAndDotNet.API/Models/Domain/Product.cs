namespace CrudByNgAndDotNet.API.Models.Domain
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Category CategoryId { get; set; } // Navigation property to Category
    }
}
