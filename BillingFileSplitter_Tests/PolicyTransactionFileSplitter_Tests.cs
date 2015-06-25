using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingFileSplitter;
using Xunit;

namespace BillingFileSplitter_Tests
{
    public class PolicyTransactionFileSplitter_Tests
    {
        [Fact(DisplayName = "AddIfNotExists Works")]
        public void AddIfNotExists_Works()
        {
            var list = new List<string>();

            PolicyTransactionFileSplitter.AddIfNotExists(list, "abc");
            Assert.Equal(1, list.Count());

            PolicyTransactionFileSplitter.AddIfNotExists(list, "def");
            Assert.Equal(2, list.Count());

            PolicyTransactionFileSplitter.AddIfNotExists(list, "abc");
            Assert.Equal(2, list.Count());
        }

        [Fact(DisplayName = "Splitter Sets Paths")]
        public void Splitter_Sets_Paths()
        {
            var splitter = new PolicyTransactionFileSplitter();

            Assert.Equal(@"C:\Dev\jstrope\_TestFiles\Billing-Adapter\Split\PLM_Accounts.txt", splitter.AccountFilePath);
            Assert.Equal(@"C:\Dev\jstrope\_TestFiles\Billing-Adapter\Split\PLM_Policies.txt", splitter.PolicyFilePath);
            Assert.Equal(@"C:\Dev\jstrope\_TestFiles\Billing-Adapter\Split\PLM_Transactions.txt", splitter.TransactionFilePath);
        }
    }
}
