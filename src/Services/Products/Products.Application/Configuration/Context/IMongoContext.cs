namespace Products.Application.Configuration.Context;

public interface IMongoContext : IDisposable
{
    void AddCommand(Func<Task> func, IEnumerable<IDomainEvent> events);
    Task<int> SaveChanges();
    IMongoCollection<T> GetCollection<T>(string name);
}
