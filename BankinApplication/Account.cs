using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApplication
{
    abstract class Account
    {
        public static int NosAccount{ get; private set; }
        public readonly Customer Customer;
        public string AccountNos { get; }
        public string AccountName { get; set; }
        public double Balance { get; private set; }
        public DateTime Date { get; } = DateTime.Now;
        List<Transaction> transactionHistory = new List<Transaction>();
        int ChargeTime { get; set; }
        double ChargeRate { get; set; }
        int InterestTime { get; set; }
        double InterestRate { get; set; }
        double MinBalance { get; set; }
        double MaxBalance { get; set; }

        Account(Customer customer, double amount)
        {
            if (amount < MinBalance || amount > MaxBalance) throw new ArgumentException("Inavalid amount");
            this.Customer = customer;
            AccountNos = generateAcctNumber();
            this.AccountName = customer.Name;
            this.deposit(amount);
            NosAccount += 1;

        }

        Account(Account account)
        {

        }

        private void update()
        {

        }

        public TransactionResult deposit(double amount)
        {
            if (amount >= 0 && Balance+amount <= MaxBalance)
            {
                Balance += amount;
                return TransactionResult.Success;
            }
            return TransactionResult.BalanceLimit;
        }

        public TransactionResult withdraw(double amount)
        {
            if (amount >= 0 && Balance-amount >= MinBalance )
            {
                Balance -= amount;
                return TransactionResult.Success;
            }
            return TransactionResult.InsufficientFunds;
        }

        public bool charge(double charge)
        {
            if (charge >= 0)
            {
                Balance -= charge;
                return true;
            }
            return false;
        }

        public bool bonus(double bonus)
        {
            if (bonus >= 0)
            {
                Balance += bonus;
                return true;
            }
            return false;
        }

        public void addTransaction(Transaction transation)
        {
            transactionHistory.Add(transation);
        }

        private string generateAcctNumber()
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
    }
}
