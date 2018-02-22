using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApplication
{
    class Customer
    {
        public string Name {get; set; }
        private List<Account> mAccounts;
        public string mAddress { get; set;}
        public DateTime mDateCreated { get; }
        Customer(String name, List<Account> accounts, string address, DateTime dateCreated)
        {
            Name = name;
            mAddress = address;
            mDateCreated = dateCreated;
            mAccounts = new List<Account>(accounts);

        }
        public void addAccount(Account acc)
        {
            mAccounts.Add(acc);
        }

    }
}
