using GameStore.Interfaces.InterfaceRepositories;
using Microsoft.EntityFrameworkCore;
using System;

namespace GameStore.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext Context { get; }
        void SaveChange();
        IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IDeveloperRepository DeveloperRepository { get; }
        IProductCategoryRepository ProductCategoryRepository { get; }
        IAccountRepository AccountRepository { get; }
        IFundRepository FundRepository { get; }
        ITransactionHistoryRepository TransactionHistoryRepository { get; }
    }
}
