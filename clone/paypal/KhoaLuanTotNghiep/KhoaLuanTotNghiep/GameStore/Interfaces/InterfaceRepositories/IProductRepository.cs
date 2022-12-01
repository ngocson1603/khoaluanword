using GameStore.Models;
using GameStore.ViewModels;
using System.Collections.Generic;

namespace GameStore.Interfaces.InterfaceRepositories
{
    public interface IProductRepository : IGameStoreRepository<Product>
    {
        public List<ProductDetail> GetProductWithCategories(int id = 0);
        public List<Product> GetProductByName(string name);
    }
}
