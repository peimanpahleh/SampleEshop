namespace Products.Infrastructure.Context;

public class MongoContext : IMongoContext
{
    private IMongoDatabase Database { get; set; }
    public IClientSessionHandle Session { get; set; }
    public IMongoClient MongoClient { get; set; }
    private readonly List<Func<Task>> _commands;
    private readonly List<IDomainEvent> _events;
    private readonly IMediator _mediator;
    public MongoContext(IMongoDatabase db, IMediator mediator)
    {
        Database = db;
        _mediator = mediator;

        MongoClient = db.Client;
        // Every command will be stored and it'll be processed at SaveChanges
        _commands = new List<Func<Task>>();
        _events = new List<IDomainEvent>();

    }


    public async Task<int> SaveChanges()
    {
        try
        {
            var count = _commands.Count;
            var eventsCount = _events.Count;
            using (Session = await MongoClient.StartSessionAsync())
            {
                Session.StartTransaction();

                var commandTasks = _commands.Select(c => c());

                await Task.WhenAll(commandTasks);

                await Session.CommitTransactionAsync();
            }

            var tempEvents = _events.ToList();
            _events.Clear();
            _commands.Clear();


            foreach (var domainEvent in tempEvents)
            {
                await _mediator.Publish(domainEvent);
            }
            return count;
        }
        catch (Exception ex)
        {

            throw;
        }
    }


    public void AddCommand(Func<Task> func, IEnumerable<IDomainEvent> events)
    {
        if (events != null)
            _events.AddRange(events);

        _commands.Add(func);
    }

    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return Database.GetCollection<T>(name);
    }

    public void Dispose()
    {
        Session?.Dispose();
        GC.SuppressFinalize(this);
    }
}
