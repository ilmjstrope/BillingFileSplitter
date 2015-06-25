using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingFileSplitter;
using Xunit;

namespace BillingFileSplitter_Tests
{
    public class PolicyTransactionFileSplitter_Split_Tests
    {
        private PolicyTransactionFileSplitter _splitter;

        private FileGroup _transactionsFileGroup;
        private FileGroup _policiesFileGroup;
        private FileGroup _accountsFileGroup;

        #region Test Initialization

        public PolicyTransactionFileSplitter_Split_Tests()
        {
            var transactionRecords = new string[5];

            transactionRecords[0] = "        |17G0120216|20150630|20150624|20150630|    64048.00|01|N|        0.00"; // New/Ren
            transactionRecords[1] = "        |35P0010116|20150630|20150624|20150630|   109280.00|02|N|        0.00"; // New/Ren
            transactionRecords[2] = "        |35P0010216|20150630|20150624|20150630|    46146.00|03|N|        0.00"; // All Else
            transactionRecords[3] = "        |29B2050115|20150621|20150624|20150621|    17904.00|04|N|        0.00"; // Canc/Rein
            transactionRecords[4] = "        |37R2010115|20150630|20150624|20150630|   150314.00|05|N|        0.00"; // Canc/Rein

            var policyRecords = new string[5];

            policyRecords[0] = "17G012  |17G0120216|20150630|20170630|02|03|MULTIYR10DOWN20PAY       |Y|0.1000|          ";
            policyRecords[1] = "35P001  |35P0010116|20150630|20170630|02|02|MULTIYR10DOWN20PAY       |Y|0.1000|          ";
            policyRecords[2] = "35P001  |35P0010216|20150630|20170630|02|03|MULTIYR10DOWN20PAY       |Y|0.1000|          ";
            policyRecords[3] = "29B205  |29B2050115|20150621|20160621|02|02|MONTHLY7PAY              |N|0.1000|          ";
            policyRecords[4] = "37R201  |37R2010115|20150630|20160630|02|02|MONTHLY7PAY              |N|0.1000|          ";

            var accountRecords = new string[4];

            accountRecords[0] = "17G012  |Some Insured 1                                     |                                                   |111 West Street                                    |                              |Ponchatoula                  |LA|70454|    |USA                 |                              |          |          |                                                  |1234  |Agency 1                                |09|                                                            |N|                                        |                             |  |     |    ";
            accountRecords[1] = "35P001  |Some Insured 2                                     |                                                   |112 West Street                                    |                              |Claremore                    |OK|74017|    |USA                 |                              |          |          |                                                  |5678  |Agency 2                                |09|                                                            |N|                                        |                             |  |     |    ";
            accountRecords[2] = "29B205  |Some Insured 3                                     |                                                   |113 West Street                                    |                              |Hammonton                    |NJ|08037|    |USA                 |                              |          |          |                                                  |9012  |Agency 3                                |09|                                                            |N|                                        |                             |  |     |    ";
            accountRecords[3] = "37R201  |Some Insured 4                                     |                                                   |114 West Street                                    |                              |Lehighton                    |PA|18235|    |USA                 |                              |          |          |                                                  |9012  |Agency 4                                |09|                                                            |N|                                        |                             |  |     |    ";

            _transactionsFileGroup = new FileGroup("x", false);
            _policiesFileGroup = new FileGroup("x", false);
            _accountsFileGroup = new FileGroup("x", false);

            _splitter = new PolicyTransactionFileSplitter();

            _splitter.SplitTransactionRecords(_transactionsFileGroup, transactionRecords);
            _splitter.SplitPolicyRecords(_policiesFileGroup, policyRecords);
            _splitter.SplitAccountRecords(_accountsFileGroup, accountRecords);
        }

        #endregion

        #region Transactions

        [Fact(DisplayName = "Splits Transaction Records")]
        public void SplitsTransactionRecords()
        {
            // For Transactions, the records' unique keys are the PolicyNumbers

            var policyNumbers_From_TransactionsGroup_NewRen = GetUniqueKeysFromTransactionRecords(_transactionsFileGroup.NewRenRecords);
            Assert.Equal("17G0120216,35P0010116", policyNumbers_From_TransactionsGroup_NewRen);

            var policyNumbers_From_TransactionsGroup_CanRein = GetUniqueKeysFromTransactionRecords(_transactionsFileGroup.CanReinRecords);
            Assert.Equal("29B2050115,37R2010115", policyNumbers_From_TransactionsGroup_CanRein);

            var policyNumbers_From_TransactionsGroup_AllElse = GetUniqueKeysFromTransactionRecords(_transactionsFileGroup.AllElseRecords);
            Assert.Equal("35P0010216", policyNumbers_From_TransactionsGroup_AllElse);
        }

        private string GetUniqueKeysFromTransactionRecords(List<string> transactionRecords)
        {
            return GetValuesFromRecords(transactionRecords, 9, 10);
        }

        #endregion

        #region Policies

        [Fact(DisplayName = "Splits Policy Records")]
        public void SplitsPolicyRecords()
        {
            var policyNumbers_From_PoliciesGroup_NewRen = GetUniqueKeysFromPolicyRecords(_policiesFileGroup.NewRenRecords);
            Assert.Equal("17G0120216,35P0010116", policyNumbers_From_PoliciesGroup_NewRen);

            var policyNumbers_From_PoliciesGroup_CanRein = GetUniqueKeysFromPolicyRecords(_policiesFileGroup.CanReinRecords);
            Assert.Equal("29B2050115,37R2010115", policyNumbers_From_PoliciesGroup_CanRein);

            var policyNumbers_From_PoliciesGroup_AllElse = GetUniqueKeysFromPolicyRecords(_policiesFileGroup.AllElseRecords);
            Assert.Equal("35P0010216", policyNumbers_From_PoliciesGroup_AllElse);
        }

        private string GetUniqueKeysFromPolicyRecords(List<string> policyRecords)
        {
            return GetValuesFromRecords(policyRecords, 9, 10);
        }

        #endregion

        #region Accounts

        [Fact(DisplayName = "Splits Account Records")]
        public void SplitsAccountRecords()
        {
            // Account 35P001 has transactions in 2 categories, so it will appear in the New/Ren and AllElse groups

            var accountNumbers_From_AccountsGroup_NewRen = GetUniqueKeysFromAccountRecords(_accountsFileGroup.NewRenRecords);
            Assert.Equal("17G012,35P001", accountNumbers_From_AccountsGroup_NewRen);

            var accountNumbers_From_AccountsGroup_CanRein = GetUniqueKeysFromAccountRecords(_accountsFileGroup.CanReinRecords);
            Assert.Equal("29B205,37R201", accountNumbers_From_AccountsGroup_CanRein);

            var accountNumbers_From_AccountsGroup_AllElse = GetUniqueKeysFromAccountRecords(_accountsFileGroup.AllElseRecords);
            Assert.Equal("35P001", accountNumbers_From_AccountsGroup_AllElse);
        }

        private string GetUniqueKeysFromAccountRecords(List<string> accountRecords)
        {
            return GetValuesFromRecords(accountRecords, 0, 6);
        }

        #endregion

        private string GetValuesFromRecords(List<string> records, int start, int length)
        {
            var values = records.AsQueryable().Select(q => q.Substring(start, length));
            return string.Join(",", values);
        }

    }
}
