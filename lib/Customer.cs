using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApplication
{
    /// <summary>
    /// A class that encapsulates the customer properties
    /// </summary>
    [Serializable]
    public class Customer
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public String Title {get; set;}
        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The name of the l.
        /// </value>
        public string LName { get; set; }
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The name of the f.
        /// </value>
        public string FName {get; set; }
        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        public Gender Gender { get; set; }

        private List<Account> accounts;
        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public string Address { get; set;}
        /// <summary>
        /// Gets the date the customer was created.
        /// </summary>
        /// <value>
        /// The date created.
        /// </value>
        public DateTime DateCreated { get; }
        /// <summary>
        /// Initializes a new instance of the <see cref="Customer"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="fName">First Name.</param>
        /// <param name="lName">Last Name.</param>
        /// <param name="gender">The gender.</param>
        /// <param name="address">The address.</param>
        /// <param name="dateCreated">The date created.</param>
        public Customer(String title, String fName, String lName, Gender gender, string address, DateTime dateCreated)
        {
            Title = title;
            FName = fName;
            LName = lName;
            Gender = gender;
            Address = address;
            DateCreated = dateCreated;
            accounts = new List<Account>();

        }
        public static void main(String[] args)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Customer"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="fName">First Name.</param>
        /// <param name="lName">Last Name.</param>
        /// <param name="gender">The gender.</param>
        /// <param name="accounts">The accounts associated with the customer.</param>
        /// <param name="address">The address.</param>
        /// <param name="dateCreated">The date created.</param>
        public Customer(String title, String fName, String lName, Gender gender, List<Account> accounts, string address, DateTime dateCreated)
        {
                Title = title;
                FName = fName;
                LName = lName;
                Gender = gender;
                Address = address;
            DateCreated = dateCreated;
            this.accounts = new List<Account>(accounts);

        }
        /// <summary>
        /// Associates an account with this customer.
        /// </summary>
        /// <param name="acc">The acc.</param>
        public void addAccount(Account acc)
        {
            accounts.Add(acc);
            
        }

        /// <summary>
        /// Gets the accounts associated with the customer.
        /// </summary>
        /// <returns></returns>
        public Account[] getAccounts()
        {
            Account[] accounts = new Account[this.accounts.Count];
            this.accounts.CopyTo(accounts);
            return accounts;
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
                string accounts = "";
                foreach (Account account in this.accounts)
                    accounts += account.AccountNos +", ";
                return accounts;
            });

            string ret = string.Format("----Customer Details----- \nName: {0} {2} {1} \nGender: {3} \nAddress: {4} \nDateCreated: {5} \nAccount Numbers: {6}", Title, FName, LName, Gender, Address, DateCreated, x());
            return ret;
        }

    }

    /// <summary>
    /// An enumeration of the gender attrib.
    /// </summary>
    public enum Gender
    {
        /// <summary>
        /// Indicates a male gender.
        /// </summary>
        Male,
        /// <summary>
        /// Indicates a female gender.
        /// </summary>
        Female
    }
}
