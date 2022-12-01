using GameStore.Context;
using GameStore.Interfaces;
using GameStore.Interfaces.InterfaceRepositories;
using GameStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Repositories
{
    public class FundRepository : GameStoreRepository<Fund>, IFundRepository
    {
        public FundRepository(GameStoreDbContext context) : base(context)
        {

        }
    }
}
