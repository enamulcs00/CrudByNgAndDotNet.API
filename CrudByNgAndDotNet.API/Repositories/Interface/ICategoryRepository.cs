using CrudByNgAndDotNet.API.Models.Domain;

namespace CrudByNgAndDotNet.API.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task<Category> CreateAsync(Category category);

        Task<IEnumerable<Category>> GetAllAsync(
            string? query = null, 
            string? sortBy = null, 
            string? sortDirection = null,
            int? pageNumber = 1,
            int? pageSize = 100);

        Task<Category?> GetById(Guid id);

        Task<Category?> UpdateAsync(Category category);

        Task<Category?> DeleteAsync(Guid id);

        Task<int> GetCount();
    }

    //Product repository interface

    public interface IProductRepository
    {
        Task<Product> CreateAsync(Product product);

        Task<IEnumerable<Product>> GetAllAsync(
            string? query = null,
            string? sortBy = null,
            string? sortDirection = null,
            int? pageNumber = 1,
            int? pageSize = 100);

        Task<Product?> GetById(Guid id);

        Task<Product?> UpdateAsync(Product product);

        Task<Product?> DeleteAsync(Guid id);
    }
}
