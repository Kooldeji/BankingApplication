using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApplication
{
    /// <summary>
    /// An account base class class that is made for savings.
    /// </summary>
    /// <seealso cref="BankingApplication.Account" />
    [Serializable]
    public class SavingsAccount : Account
    {
        static SavingsAccount()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SavingsAccount"/> class.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <param name="amount">The amount to start the account with.</param>
        public SavingsAccount(Customer customer, Double amount)
            : base(customer, amount, 1000, 1000000, 30, 0, 30, 100)
        {
        }

        public override string AccountType { get; } = "Savings ACCOUNT";
    }
}
