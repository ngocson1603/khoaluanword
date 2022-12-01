using GameStore.Context;
using GameStore.Interfaces;
using GameStore.Interfaces.InterfaceRepositories;
using GameStore.Models;

namespace GameStore.Repositories
{
    public class ProductCategoryRepository : GameStoreRepository<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(GameStoreDbContext context) : base(context)
        {

        }
    }
}
