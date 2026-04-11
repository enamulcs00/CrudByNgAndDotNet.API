using CrudByNgAndDotNet.API.Data;
using CrudByNgAndDotNet.API.Models.Domain;
using CrudByNgAndDotNet.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CrudByNgAndDotNet.API.Repositories.Implementation;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext dbContext;

    public ProductRepository(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Product> CreateAsync(Product product)
    {
        await dbContext.Products.AddAsync(product);
        await dbContext.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> DeleteAsync(Guid id)
    {
        var existingProduct = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (existingProduct is null)
        {
            return null;
        }

        dbContext.Products.Remove(existingProduct);
        await dbContext.SaveChangesAsync();
        return existingProduct;
    }

    public async Task<IEnumerable<Product>> GetAllAsync(
        string? query = null,
        string? sortBy = null,
        string? sortDirection = null,
        int? pageNumber = 1,
        int? pageSize = 100)
    {
        var products = dbContext.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
        {
            products = products.Where(x => x.Name.Contains(query));
        }

        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            products = sortBy.ToLower() switch
            {
                "name" => sortDirection?.ToLower() == "asc" ? products.OrderBy(x => x.Name) : products.OrderByDescending(x => x.Name),
                _ => products
            };
        }

        var skipResults = (pageNumber - 1) * pageSize;
        products = products.Skip((int)skipResults).Take(pageSize ?? 100);

        return await products.ToListAsync();
    }

    public async Task<Product?> GetById(Guid id)
    {
        return await dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Product?> UpdateAsync(Product product)
    {
        var existingProduct = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == product.Id);

        if (existingProduct != null)
        {
            dbContext.Entry(existingProduct).CurrentValues.SetValues(product);
            await dbContext.SaveChangesAsync();
            return product;
        }

        return null;
    }
}
