using CrudByNgAndDotNet.API.Models.Domain;

namespace CrudByNgAndDotNet.API.Repositories.Interface
{
    public interface IImageRepository
    {
        Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);

        Task<IEnumerable<BlogImage>> GetAll();
    }
}
