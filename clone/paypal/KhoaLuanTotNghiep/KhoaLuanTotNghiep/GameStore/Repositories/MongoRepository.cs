using GameStore.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Repositories
{
    public abstract class MongoRepository<T> : IMongoRepository<T> where T : class
    {
        protected readonly IMongoContext Context;
        protected IMongoCollection<T> DbSet;

        protected MongoRepository(IMongoContext context)
        {
            Context = context;
            DbSet = Context.GetCollection<T>(typeof(T).Name);
        }

        public void Add(T obj)
        {
            DbSet.InsertOne(obj);
        }

        public void Dispose()
        {
            Context?.Dispose();
        }

        public List<T> GetAll()
        {
            return DbSet.Find(Builders<T>.Filter.Empty).ToList();
        }

        public T GetById(string id)
        {
            return DbSet.Find(FilterId(id)).SingleOrDefault();
        }

        public void Remove(string id)
        {
            DbSet.DeleteOne(FilterId(id));
        }

        public void Update(string id, T obj)
        {
            DbSet.ReplaceOne(FilterId(id), obj);
        }
        private static FilterDefinition<T> FilterId(string key)
        {
            return Builders<T>.Filter.Eq("postID", key);
        }
    }
}
