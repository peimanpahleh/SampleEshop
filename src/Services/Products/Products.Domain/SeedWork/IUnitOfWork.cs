namespace Products.Domain.SeedWork;

public interface IUnitOfWork : IDisposable
{
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
}



