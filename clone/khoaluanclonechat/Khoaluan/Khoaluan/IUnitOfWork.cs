﻿using Khoaluan.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace Khoaluan
{
    public interface IUnitOfWork:IDisposable
    {
        DbContext Context { get; }
        void SaveChange();
        IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IDeveloperRepository DeveloperRepository { get; }   
        IProductCategoryRepository ProductCategoryRepository { get; }
        IUsersRepository UserRepository { get; }
        ILibraryRepository LibraryRepository { get; }
        IOrderRepository OrderRepository { get; }
        IInventoryRepository InventoryRepository { get; }
        IMarketRepository MarketRepository { get; }
        IRefundRepository RefundRepository { get; }
    }
}
