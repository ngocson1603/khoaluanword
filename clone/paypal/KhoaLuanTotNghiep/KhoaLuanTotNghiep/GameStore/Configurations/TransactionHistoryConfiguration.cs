using GameStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Configurations
{
    public class TransactionHistoryConfiguration : IEntityTypeConfiguration<TransactionHistory>
    {
        public void Configure(EntityTypeBuilder<TransactionHistory> builder)
        {
            builder.ToTable(nameof(TransactionHistory));
            builder.HasKey(e => e.TransactionId);
            builder.HasOne(e => e.Account).WithMany(p => p.TransactionHistories).HasForeignKey(e => e.AccountId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.Fund).WithMany(c => c.TransactionHistories).HasForeignKey(e => e.FundId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
