using Dapper;
using GameStore.Context;
using GameStore.Interfaces;
using GameStore.Interfaces.InterfaceRepositories;
using GameStore.Models;
using GameStore.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Repositories
{
    public class TransactionHistoryRepository : GameStoreRepository<TransactionHistory>, ITransactionHistoryRepository
    {
        public TransactionHistoryRepository(GameStoreDbContext context) : base(context)
        {

        }

        public List<TransactionHistoryViewModel> GetTransactionHistoriesByAccountId(int id)
        {
            var query = @"select t.TransactionId, t.PaymentId, a.Username, f.[Name], t.PurchasedDate, t.PreviousWalletBalance, t.NewWalletBalance
                        from TransactionHistory t, Account a, Fund f
                        where t.AccountId = a.Id and t.FundId = f.Id and a.Id = @id";
            var para = new DynamicParameters();
            para.Add("id", id);

            return (List<TransactionHistoryViewModel>)Context.Database.GetDbConnection().Query<TransactionHistoryViewModel>(query, para);
        }
    }
}
