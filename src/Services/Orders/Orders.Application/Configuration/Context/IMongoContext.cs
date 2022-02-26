using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Application.Configuration.Context
{
    public interface IMongoContext :  IDisposable 
    {
        void AddCommand(Func<Task> func,IEnumerable<IDomainEvent> events);
        Task<int> SaveChanges();
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
