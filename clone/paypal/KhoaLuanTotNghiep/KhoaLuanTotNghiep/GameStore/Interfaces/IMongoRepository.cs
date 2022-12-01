using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Interfaces
{
    public interface IMongoRepository<T> : IDisposable where T : class
    {
        void Add(T obj);
        T GetById(string id);
        List<T> GetAll();
        void Update(string id, T obj);
        void Remove(string id);
    }
}
