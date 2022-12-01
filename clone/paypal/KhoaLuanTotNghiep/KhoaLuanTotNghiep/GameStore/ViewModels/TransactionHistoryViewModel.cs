using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.ViewModels
{
    public class TransactionHistoryViewModel
    {
        public string TransactionId { get; set; }
        public string PaymentId { get; set; }
        public string Username { get; set; }
        public string FundName { get; set; }
        public DateTime PurchasedDate { get; set; }
        public int PreviousWalletBalance { get; set; }
        public int NewWalletBalance { get; set; }
    }
}
