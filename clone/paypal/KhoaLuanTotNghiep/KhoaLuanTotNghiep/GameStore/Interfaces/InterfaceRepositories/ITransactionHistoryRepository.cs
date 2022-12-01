using GameStore.Models;
using GameStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Interfaces.InterfaceRepositories
{
    public interface ITransactionHistoryRepository : IGameStoreRepository<TransactionHistory>
    {
        public List<TransactionHistoryViewModel> GetTransactionHistoriesByAccountId(int id);
    }
}
