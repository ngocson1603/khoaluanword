using GameStore.Interfaces;
using GameStore.Interfaces.InterfaceRepositories;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Context
{
    public class UnitOfWork : IUnitOfWork
    {
        public DbContext Context { get; }
        public IProductCategoryRepository ProductCategoryRepository { get; set; }
        public IProductRepository ProductRepository { get; set; }
        public ICategoryRepository CategoryRepository { get; set; }
        public IDeveloperRepository DeveloperRepository { get; set; }
        public IAccountRepository AccountRepository { get; set; }
        public IFundRepository FundRepository { get; set; }
        public ITransactionHistoryRepository TransactionHistoryRepository { get; set; }

        public UnitOfWork(GameStoreDbContext context, 
            IProductCategoryRepository productCategoryRepository, 
            IProductRepository productRepository, 
            ICategoryRepository categoryRepository, 
            IDeveloperRepository developerRepository,
            IAccountRepository accountRepository,
            IFundRepository fundRepository,
            ITransactionHistoryRepository transactionHistoryRepository
            )
        {
            Context = context;
            ProductCategoryRepository = productCategoryRepository;
            ProductRepository = productRepository;
            CategoryRepository = categoryRepository;
            DeveloperRepository = developerRepository;
            AccountRepository = accountRepository;
            FundRepository = fundRepository;
            TransactionHistoryRepository = transactionHistoryRepository;
        }

        public void Dispose()
        {
            Context?.Dispose();
        }

        public void SaveChange()
        {
            Context.SaveChanges();
        }
    }
}
