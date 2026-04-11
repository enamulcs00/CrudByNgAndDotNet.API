using CrudByNgAndDotNet.API.Models.Domain;

namespace CrudByNgAndDotNet.API.Models.DTO
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Category CategoryId { get; set; }
    }
}
