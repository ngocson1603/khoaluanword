using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models
{
    public class TransactionHistory
    {
        public string TransactionId { get; set; }
        public string PaymentId { get; set; }
        public int AccountId { get; set; }
        public int FundId { get; set; }
        public DateTime PurchasedDate { get; set; }
        public int PreviousWalletBalance { get; set; }
        public int NewWalletBalance { get; set; }
        public virtual Account Account { get; set; }
        public virtual Fund Fund { get; set; }
    }
}
