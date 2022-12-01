using GameStore.Context;
using GameStore.Interfaces;
using GameStore.Interfaces.InterfaceRepositories;
using GameStore.Models;

namespace GameStore.Repositories
{
    public class DeveloperRepository : GameStoreRepository<Developer>, IDeveloperRepository
    {
        public DeveloperRepository(GameStoreDbContext context) : base(context)
        {

        }
    }
}
