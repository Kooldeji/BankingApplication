using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApplication
{
    /// <summary>
    /// A class that handles a transaction
    /// </summary>
    [Serializable]
    public class Transaction
    {
        /// <summary>
        /// The accounts involved in the transaction
        /// </summary>
        Account[] Accounts = { };
        /// <summary>
        /// Gets the amount involved in the transaction.
        /// </summary>
        /// <value>
        /// The amount involved in the transaction.
        /// </value>
        public double Amount { get; private set; }
        /// <summary>
        /// Gets the date of the transaction.
        /// </summary>
        /// <value>
        /// The date of the transaction.
        /// </value>
        public DateTime Date { get; } = DateTime.Now;
        /// <summary>
        /// Gets or sets the charge rate.
        /// </summary>
        /// <value>
        /// The charge rate.
        /// </value>
        public ETransactionResult Result { get; private set; } = ETransactionResult.Incomplete;
        /// <summary>
        /// Gets the type of the transaction.
        /// </summary>
        /// <value>
        /// The type of the transaction.
        /// </value>
        public ETransactionType TransactionType { get; } 
        string TransactionMessage
        {
            get
            {
                switch (Result)
                {
                    case ETransactionResult.BalanceLimit:
                        return "Transaction Failed due to Balance limit Reached.";
                    case ETransactionResult.InsufficientFunds:
                        return "Transaction Failed due to Insufficient Funds.";
                    case ETransactionResult.Unknown:
                        return "Transaction failed due to unknown reasons.";
                    case ETransactionResult.Success:
                        return "Transaction Succesfull";
                    case ETransactionResult.Incomplete:
                        return "Incomplete Transaction";
                }
                return "";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transaction"/> class.
        /// </summary>
        /// <param name="amount">The amount to be used for the transaction</param>
        /// <param name="transactionType">The type of the transaction.</param>
        /// <param name="accounts">The accounts involved in the transaction.</param>
        public Transaction(double amount, ETransactionType transactionType, params Account[] accounts)
        {
            Amount = amount;
            TransactionType = transactionType;
            Accounts = new Account[accounts.Length];
            Array.Copy(accounts, Accounts, accounts.Length);
        }

        /// <summary>
        /// Commutes the Transaction.
        /// </summary>
        public void transact()
        {
            try {
                switch (TransactionType)
                {
                    case ETransactionType.Withdrawal:
                        Result = Accounts[0].Withdraw(Amount);
                        Accounts[0].AddTransaction(this);
                        break;
                    case ETransactionType.Deposit:
                        Result = Accounts[0].Deposit(Amount);
                        Accounts[0].AddTransaction(this);
                        break;
                    case ETransactionType.Transfer:
                        Result = Accounts[0].Withdraw(Amount);
                        if (Result == ETransactionResult.Success)
                        {
                            Result = Accounts[1].Deposit(Amount);
                        }
                        Accounts[0].AddTransaction(this);
                        Accounts[1].AddTransaction(this);
                        break;
                    case ETransactionType.Loan:
                        Result = Accounts[0].Deposit(Amount);
                        Accounts[0].AddTransaction(this);
                        break;

                }
            }catch(IndexOutOfRangeException e)
            {
                throw;
            }
        }
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var x = new Func<Object>(() => {
                string transactions = "";
                foreach (Account account in this.Accounts)
                    transactions += account.AccountName + ", ";
                return transactions;
            });

            string ret = string.Format("Transaction Date: {0} \nTransaction Type: {1} \nAmount: {2} \nTransaction Result: {3} \nAccounts Involved: {4}", Date, TransactionType, Amount, TransactionMessage, x());
            return ret;
        }
    }

    /// <summary>
    /// An enum of the types of transactions possible.
    /// </summary>
    public enum ETransactionType
    {
        /// <summary>
        /// Indicates a transfer transaction.
        /// </summary>
        Transfer,
        /// <summary>
        /// Indicates a deposit transaction.
        /// </summary>
        Deposit,
        /// <summary>
        /// Indictates a Withdrawal transaction.
        /// </summary>
        Withdrawal,
        /// <summary>
        /// Indicates a loan transaction.
        /// </summary>
        Loan
    }
    /// <summary>
    /// An enum of the results of a transaction.
    /// </summary>
    public enum ETransactionResult
    {
        /// <summary>
        /// Indicates a transaction success
        /// </summary>
        Success,
        /// <summary>
        /// Indicates a transaction failure due to  InsufficientFunds
        /// </summary>
        InsufficientFunds,
        /// <summary>
        /// Indicates a transaction failure due to balance limit
        /// </summary>
        BalanceLimit,
        /// <summary>
        /// Indicates an transaction failure due to an unknown cause
        /// </summary>
        Unknown,
        /// <summary>
        /// Indicates an incomplete transaction
        /// </summary>
        Incomplete
    }
}
