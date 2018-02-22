using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApplication
{
    /// <summary>
    /// An checking base account class.
    /// </summary>
    /// <seealso cref="BankingApplication.Account" />
    [Serializable]
    public class CurrentAccount : Account
    {
        /// <summary>
        /// Gets or sets the overdraft.
        /// </summary>
        /// <value>
        /// The overdraft.
        /// </value>
        public double Overdraft { get; set; }
        /// <summary>
        /// Gets the balance of the account that must be in the range of the min and max balance.
        /// </summary>
        /// <value>
        /// The balance.
        /// </value>
        public override double Balance
        {
            get
            {
                return base.Balance-Overdraft;
            }
        }

        public override string AccountType { get; } = "Current Account";

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentAccount"/> class.
        /// </summary>
        /// <param name="cust">The customer.</param>
        /// <param name="amount">The amount.</param>
        public CurrentAccount(Customer cust, Double amount)
            :base(cust, amount, 150000, 100000000, 0, 0, 30, 100)
        {
        }
        /// <summary>
        /// Withdraws the specified amount.
        /// </summary>
        /// <param name="amount">The amount to Withdraw.</param>
        /// <returns></returns>
        internal override ETransactionResult Withdraw(double amount)
        {
            if (Balance-amount< MinBalance)
            {
                double overdraft = Balance - MinBalance;
                amount -= overdraft;
                Overdraft += amount;
            }
            return base.Withdraw(amount);
        }
        /// <summary>
        /// Deposits the specified amount.
        /// </summary>
        /// <param name="amount">The amount to deposit.</param>
        /// <returns>
        /// An EtransactionResult
        /// </returns>
        internal override ETransactionResult Deposit(double amount)
        {
            if (Overdraft >=0)
            {
                double overdraft = Overdraft - amount;
                if (overdraft <0)
                {
                    Overdraft = overdraft;
                    return ETransactionResult.Success;
                }
                else
                {
                    amount = overdraft;
                }
            }
            return base.Deposit(amount);
        }


    }
}
