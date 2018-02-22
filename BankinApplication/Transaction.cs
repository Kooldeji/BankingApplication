using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApplication
{
    class Transaction
    {
        Account[] Accounts = { };
        public double Amount { get; private set; }
        public DateTime Date { get; } = DateTime.Now;
        public double ChargeRate { get; set; }
        public double InterestRate { get; set; } 
        public TransactionResult Result { get; private set; }
        public TransactionType TransactionType { get; }
        string TransactionMessage
        {
            get
            {
                switch (Result)
                {
                    case TransactionResult.BalanceLimit:
                        return "Transaction Failed due to Balance limit Reached.";
                    case TransactionResult.InsufficientFunds:
                        return "Transaction Failed due to Insufficient Funds.";
                    case TransactionResult.Unknown:
                        return "Transaction failed due to unknown reasons.";
                    case TransactionResult.Success:
                        return "Transaction Succesfull";
                    case TransactionResult.Incomplete:
                        return "Incomplete Transaction";
                }
                return "";
            }
        }

        Transaction(double amount, TransactionType transactionType, double chargeRate, double interestRate, params Account[] accounts)
        {
            Amount = amount;
            TransactionType = transactionType;
            Array.Copy(Accounts, accounts, accounts.Length);
            ChargeRate = chargeRate;
            InterestRate = interestRate;
        }

        public void transact()
        {
            try {
                switch (TransactionType)
                {
                    case TransactionType.Withdrawal:
                        Result = Accounts[0].withdraw(Amount);
                        if (Result == TransactionResult.Success) update();
                        Accounts[0].addTransaction(this);
                        break;
                    case TransactionType.Deposit:
                        Result = Accounts[0].deposit(Amount);
                        if (Result == TransactionResult.Success) update();
                        Accounts[0].addTransaction(this);
                        break;
                    case TransactionType.Transfer:
                        Result = Accounts[0].withdraw(Amount);
                        if (Result == TransactionResult.Success)
                        {
                            Result = Accounts[1].deposit(Amount);
                            update();
                        }
                        Accounts[0].addTransaction(this);
                        Accounts[1].addTransaction(this);
                        break;
                    case TransactionType.Loan:
                        Result = Accounts[0].deposit(Amount);
                        if (Result == TransactionResult.Success) update();
                        Accounts[0].addTransaction(this);
                        break;

                }
            }catch(IndexOutOfRangeException e)
            {
                throw;
            }
        }

        private void update()
        {
            
        }
    }

    enum TransactionType
    {
        Transfer,
        Deposit,
        Withdrawal,
        Loan
    }
    enum TransactionResult
    {
        Success,
        InsufficientFunds,
        BalanceLimit,
        Unknown,
        Incomplete
    }
}
