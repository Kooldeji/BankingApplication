using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApplication
{
    /// <summary>
    /// An abstract account API that defines overidable methods for derived account types.
    /// </summary>
    [Serializable]
    public abstract class Account
    {
        /// <summary>
        /// Gets or sets the type of the account.
        /// </summary>
        /// <value>
        /// The type of the account.
        /// </value>
        public abstract string AccountType { get;}
        /// <summary>
        /// Gets or sets the nos accounts created.
        /// </summary>
        /// <value>
        /// The nos account.
        /// </value>
        public static int NosAccount { get; set; }
        /// <summary>
        /// The customer associated with this account.
        /// </summary>
        public readonly Customer Customer;
        /// <summary>
        /// Gets the account nos associated with the account.
        /// </summary>
        /// <value>
        /// The account nos.
        /// </value>
        public string AccountNos { get; }
        /// <summary>
        /// Gets or sets the name of the account.
        /// </summary>
        /// <value>
        /// The name of the account.
        /// </value>
        public virtual string AccountName { get; set; }
        /// <summary>
        /// Gets the balance of the account that must be in the range of the min and max balance.
        /// </summary>
        /// <value>
        /// The balance.
        /// </value>
        public virtual double Balance { get; internal set; }
        /// <summary>
        /// Gets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date { get; } = DateTime.Now;
        List<Transaction> transactionHistory = new List<Transaction>();
        /// <summary>
        /// Gets or sets the time interval for charging.
        /// </summary>
        /// <value>
        /// The charge time.
        /// </value>
        public int ChargeTime { get; }
        /// <summary>
        /// Gets or sets the charge amount.
        /// </summary>
        /// <value>
        /// The charge.
        /// </value>
        public double ChargeValue { get; }
        /// <summary>
        /// Gets the time interval for interest.
        /// </summary>
        /// <value>
        /// The interest time.
        /// </value>
        public int InterestTime { get; }
        /// <summary>
        /// Gets the interest amount.
        /// </summary>
        /// <value>
        /// The interest.
        /// </value>
        public double InterestValue { get;}
        /// <summary>
        /// Gets the minimum balance allowable.
        /// </summary>
        /// <value>
        /// The minimum balance.
        /// </value>
        public double MinBalance { get; }
        /// <summary>
        /// Gets the maximum balance allowable.
        /// </summary>
        /// <value>
        /// The maximum balance.
        /// </value>
        public double MaxBalance { get;}

        /// <summary>
        /// Gets or sets the last updated date.
        /// </summary>
        /// <value>
        /// The last updated date.
        /// </value>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Account"/> class.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <param name="amount">The amount to start the account with.</param>
        /// <param name="minBalance">The minimum balance.</param>
        /// <param name="maxBalance">The maximum balance.</param>
        /// <exception cref="System.ArgumentException">Inavalid amount</exception>
        public Account(Customer customer, double amount, double minBalance, double maxBalance)
        {
            if (minBalance >= maxBalance) throw new ArgumentException();
            if (amount < minBalance || amount > maxBalance) throw new ArgumentException("Inavalid amount");
            if (customer == null) throw new ArgumentException("Inavalid Customer");
            this.MinBalance = minBalance;
            this.MaxBalance = maxBalance;
            this.Customer = customer;
            AccountNos = GenerateAcctNumber();
            this.AccountName = customer.FName+" "+customer.LName;
            this.Deposit(amount);
            NosAccount += 1;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Account"/> class.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="minBalance">The minimum balance.</param>
        /// <param name="maxBalance">The maximum balance.</param>
        /// <param name="interestTime">The interest time.</param>
        /// <param name="interestValue">The interest value.</param>
        /// <param name="chargeTime">The charge time.</param>
        /// <param name="chargeValue">The charge value.</param>
        public Account(Customer customer, double amount, double minBalance, double maxBalance, int interestTime, double interestValue, int chargeTime, double chargeValue)
            :this(customer, amount, minBalance, maxBalance)
        {
            this.InterestTime = interestTime;
            this.InterestValue = interestValue;
            this.ChargeTime = chargeTime;
            this.ChargeValue = chargeValue;
        }

        /// <summary>
        /// Updates the account to give interest and charge.
        /// </summary>
        public virtual void Update()
        {
            int days = (int)(DateTime.Now.Ticks - LastUpdated.Ticks / 24);
            Balance += (days / InterestTime) * InterestValue;
            Balance -= (days / ChargeTime) * ChargeValue;
        }

        /// <summary>
        /// Deposits the specified amount.
        /// </summary>
        /// <param name="amount">The amount to deposit.</param>
        /// <returns>An EtransactionResult</returns>
        internal virtual ETransactionResult Deposit(double amount)
        {
            if (amount+Balance >= MinBalance && Balance + amount <= MaxBalance)
            {
                Balance += amount;
                return ETransactionResult.Success;
            }
            return ETransactionResult.BalanceLimit;
        }

        /// <summary>
        /// Withdraws the specified amount.
        /// </summary>
        /// <param name="amount">The amount to Withdraw.</param>
        /// <returns></returns>
        internal virtual ETransactionResult Withdraw(double amount)
        {
            if (Balance-amount >= MinBalance )
            {
                Balance -= amount;
                return ETransactionResult.Success;
            }
            return ETransactionResult.InsufficientFunds;
        }

        /// <summary>
        /// Adds the transaction to the ACCOUNT's transaction history.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        internal void AddTransaction(Transaction transaction)
        {
            transactionHistory.Add(transaction);
        }

        /// <summary>
        /// Gets the transactions.
        /// </summary>
        /// <returns>An array copy of the transactions</returns>
        public Transaction[] GetTransactions()
        {
            Transaction[] transactionHistory = new Transaction[this.transactionHistory.Count];
            this.transactionHistory.CopyTo(transactionHistory);
            return transactionHistory;
        }

        /// <summary>
        /// Generates an acct number for the account.
        /// </summary>
        /// <returns></returns>
        protected virtual string GenerateAcctNumber() 
        {
            int ret = 101121145;
            ret += NosAccount;
            return "" + ret;
        }

        static void Main(string[] args)
        {
            DateTime date = DateTime.Now;
            Console.Write(date);
            Console.ReadLine();
        }
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {

            string ret = string.Format("{4}\nAccount Name: {0} \nAccount Number: {1} \nDateCreated: {2} \nBalance : {3}", AccountName, AccountNos, Date, Balance, AccountType);
            return ret;
        }
    }
}
