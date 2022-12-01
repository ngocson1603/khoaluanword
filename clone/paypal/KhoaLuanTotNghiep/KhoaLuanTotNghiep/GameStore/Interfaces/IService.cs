using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameStore.Interfaces.InterfaceServices;

namespace GameStore.Interfaces
{
    public interface IService
    {
        IProductService ProductService { get; }
        ICategoryService CategoryService { get; }
        IDeveloperService DeveloperService { get; }
        IProductCategoryService ProductCategoryService { get; }
        IAccountService AccountService { get; }
        IFundService FundService { get; }
        ITransactionHistoryService TransactionHistoryService { get; }
    }
}
