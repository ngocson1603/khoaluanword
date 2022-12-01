using GameStore.Configurations;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Context
{
    public class GameStoreDbContext : DbContext
    {
        public GameStoreDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new DeveloperConfiguration());
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new FundConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionHistoryConfiguration());
        }
    }
}
