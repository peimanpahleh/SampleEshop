namespace Products.Infrastructure.Context;

public class UnitOfWork : IUnitOfWork
{
    private readonly IMongoContext _context;

    public UnitOfWork(IMongoContext context)
    {
        _context = context;
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var changeAmount = await _context.SaveChanges();
        return changeAmount > 0;
    }


    public void Dispose()
    {
        _context.Dispose();
    }


}
