using GameStore.Context;
using GameStore.Interfaces;
using GameStore.Interfaces.InterfaceRepositories;
using GameStore.Models;

namespace GameStore.Repositories
{
    public class CategoryRepository : GameStoreRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(GameStoreDbContext context) : base(context)
        {

        }
    }
}
