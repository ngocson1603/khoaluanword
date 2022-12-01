using GameStore.Interfaces;
using GameStore.Interfaces.InterfaceServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Context
{
    public class Service : IService
    {
        public IProductService ProductService { get; }
        public ICategoryService CategoryService { get; }
        public IDeveloperService DeveloperService { get; }
        public IProductCategoryService ProductCategoryService { get; }
        public IAccountService AccountService { get; }
        public IFundService FundService { get; }
        public ITransactionHistoryService TransactionHistoryService { get; }

        public Service(IProductCategoryService productCategoryService,
            IProductService productService,
            ICategoryService categoryService,
            IDeveloperService developerService,
            IAccountService accountService,
            IFundService fundService,
            ITransactionHistoryService transactionHistoryService
            )
        {
            ProductCategoryService = productCategoryService;
            ProductService = productService;
            CategoryService = categoryService;
            DeveloperService = developerService;
            AccountService = accountService;
            FundService = fundService;
            TransactionHistoryService = transactionHistoryService;
        }
    }
}
