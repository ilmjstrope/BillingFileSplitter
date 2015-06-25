using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BillingFileSplitter
{
    public class PolicyTransactionFileSplitter
    {
        private string FilePath = @"C:\Dev\jstrope\_TestFiles\Billing-Adapter\Split";
        public string AccountFilePath;
        public string PolicyFilePath;
        public string TransactionFilePath;

        public List<string> NewRenPolicies = new List<string>();
        public List<string> CanReinPolicies = new List<string>();
        public List<string> AllElsePolicies = new List<string>();

        public List<string> NewRenAccounts = new List<string>();
        public List<string> CanReinAccounts = new List<string>();
        public List<string> AllElseAccounts = new List<string>();

        public PolicyTransactionFileSplitter()
        {
            AccountFilePath = Path.Combine(FilePath, "PLM_Accounts.txt");
            PolicyFilePath = Path.Combine(FilePath, "PLM_Policies.txt");
            TransactionFilePath = Path.Combine(FilePath, "PLM_Transactions.txt");
        }

        public string SplitFiles()
        {
            var checkFilesResult = CheckFiles();
            if (checkFilesResult != null)
                return checkFilesResult;

            return Split();
        }

        private string CheckFiles()
        {
            if (!File.Exists(AccountFilePath))
                return AccountFilePath + " not found";

            if (!File.Exists(PolicyFilePath))
                return PolicyFilePath + " not found";

            if (!File.Exists(TransactionFilePath))
                return TransactionFilePath + " not found";

            return null;
        }

        public string Split()
        {
            var transactionsFileGroup = new FileGroup(TransactionFilePath);
            var policiesFileGroup = new FileGroup(PolicyFilePath);
            var accountsFileGroup = new FileGroup(AccountFilePath);

            var transactionRecords = File.ReadAllLines(TransactionFilePath);
            var policyRecords = File.ReadAllLines(PolicyFilePath);
            var accountRecords = File.ReadAllLines(AccountFilePath);

            SplitTransactionRecords(transactionsFileGroup, transactionRecords);
            SplitPolicyRecords(policiesFileGroup, policyRecords);
            SplitAccountRecords(accountsFileGroup, accountRecords);

            transactionsFileGroup.SaveAllFiles();
            policiesFileGroup.SaveAllFiles();
            accountsFileGroup.SaveAllFiles();

            return "Complete";
        }

        public void SplitTransactionRecords(FileGroup transactionsFileGroup, string[] transactions)
        {
            foreach (var record in transactions)
            {
                var recordType = Convert.ToInt32(record.Substring(60, 2));
                var policyNumber = record.Substring(9, 10);
                var accountNumber = record.Substring(9, 6);

                if (recordType == 1 || recordType == 2)
                {
                    transactionsFileGroup.NewRenRecords.Add(record);
                    AddIfNotExists(NewRenPolicies, policyNumber);
                    AddIfNotExists(NewRenAccounts, accountNumber);
                }
                else if (recordType == 4 || recordType == 5)
                {
                    transactionsFileGroup.CanReinRecords.Add(record);
                    AddIfNotExists(CanReinPolicies, policyNumber);
                    AddIfNotExists(CanReinAccounts, accountNumber);
                }
                else
                {
                    transactionsFileGroup.AllElseRecords.Add(record);
                    AddIfNotExists(AllElsePolicies, policyNumber);
                    AddIfNotExists(AllElseAccounts, accountNumber);
                }
            }
        }

        public void SplitPolicyRecords(FileGroup policiesFileGroup, string[] policies)
        {
            foreach (var record in policies)
            {
                var policyNumber = record.Substring(9, 10);

                // An account can exist in multiple lists if multiple transaction types come in for that account.

                if (NewRenPolicies.Contains(policyNumber))
                    policiesFileGroup.NewRenRecords.Add(record);

                if (CanReinPolicies.Contains(policyNumber))
                    policiesFileGroup.CanReinRecords.Add(record);

                if (AllElsePolicies.Contains(policyNumber))
                    policiesFileGroup.AllElseRecords.Add(record);
            }
        }

        public void SplitAccountRecords(FileGroup accountsFileGroup, string[] accounts)
        {
            foreach (var record in accounts)
            {
                var accountNumber = record.Substring(0, 6);

                // An account can exist in multiple lists if multiple transaction types come in for that account.

                if (NewRenAccounts.Contains(accountNumber))
                    accountsFileGroup.NewRenRecords.Add(record);

                if (CanReinAccounts.Contains(accountNumber))
                    accountsFileGroup.CanReinRecords.Add(record);

                if (AllElseAccounts.Contains(accountNumber))
                    accountsFileGroup.AllElseRecords.Add(record);
            }
        }

        public static void AddIfNotExists(List<string> list, string value)
        {
            if (!list.Contains(value))
                list.Add(value);
        }
    }

}
