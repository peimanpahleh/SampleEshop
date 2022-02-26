namespace Products.Domain.Images;

public interface IImageRepository : IRepository<Image>, IDisposable
{
    void Add(Image image);
    Task<Image> GetByIdAsync(string imageId);
    Task<bool> AnyAsync(string imageId);

}
