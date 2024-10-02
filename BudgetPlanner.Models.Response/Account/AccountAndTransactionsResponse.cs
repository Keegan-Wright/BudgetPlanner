using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.Models.Response.Account
{
    public class AccountAndTransactionsResponse
    {
        public string AccountName { get; set; }

        public byte[] Logo { get; set; }

        public string AccountType { get; set; }
        public decimal AccountBalance { get; set; }

        public decimal AvailableBalance { get; set; }

        public IAsyncEnumerable<AccountTransactionResponse>? Transactions { get; set; }

    }
}
