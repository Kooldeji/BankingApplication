using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingApplication;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BankingInterface
{
    delegate bool Action(params Object[] args);

    class Program
    {
        public enum Fields { Name, Address, Gender, PhoneNos, Email, Deposit, Savings, Current}
        public static Action GENDERPREDICATE { get; } = value => String.Compare(value[0].ToString(), "male", true) == 0 || String.Compare(value[0].ToString(), "female", true) == 0;
        public static Action FUNDSPREDICATE { get; } = value => { double dummy = 0; return Double.TryParse(value[0].ToString(), out dummy); };
        public static Action ACCOUNTNOSPREDICATE { get; } = value => value[0].ToString().Length == 9;
        public static Action TEXTPREDICATE { get; } = value => value[0].ToString().All(char.IsLetter);
        public static Action ALPHANUMERICPREDICATE { get; } = value => value[0].ToString().All(char.IsLetterOrDigit);
         
        private const string PASSKEY = "RFCFFBETU";
        private const string USERNAME = "CSC322";

        Dictionary<string, Account> accounts = new Dictionary<string, Account>();
        Dictionary<string, Customer> customers = new Dictionary<string, Customer>();
        List<Transaction> transactions = new List<Transaction>();

        static void Main(string[] args)
        {
            Program bInterface = new Program();
            Console.Title = "NACOSS inc. INTERFACE ";
            bInterface.Clear();
            bInterface.showAuth();
            while (true)
            {
                bInterface.MainMenu();
                Console.Write(">Hit a key to CONTINUE<");
                Console.ReadKey();
            }
        }

        private void showAuth()
        {
            Console.WriteLine("Please supply authentication....");
            string username = input("Username", ALPHANUMERICPREDICATE);
            string passkey = input("Passkey", ALPHANUMERICPREDICATE);
            while (!(string.Compare(username, USERNAME, true) == 0) || !(string.Compare(passkey, PASSKEY, false)==0))
            {
                Console.WriteLine("Authentication Failed!");
                Console.ReadKey();
                Clear();
                username = input("Username", ALPHANUMERICPREDICATE);
                passkey = input("Passkey", TEXTPREDICATE);
            }
        }

        Program()
        {
            accounts = getAccounts();
        }
        bool MainMenu(params object[] args)
        {
            Option oCreate = new Option("Create Account",
                new Option("Create Current account", 
                            new Option("New User", createAccount, Fields.Current),
                            new Option("Existing User", exisitingUserAccount, Fields.Current)),
                new Option("Create Savings Account",
                            new Option("New User", createAccount, Fields.Savings),
                            new Option("Existing User", exisitingUserAccount, Fields.Savings)),
                /*
                new Option("Create Fixed Deposit Account",
                            new Option("New User", createAccount, Fields.Deposit),
                            new Option("Existing User", exisitingUserAccount, Fields.Deposit)),
                */
                new Option("Main menu", MainMenu));
            Option oWithdraw = new Option("Withdraw from Account", withdraw);
            Option oDeposit = new Option("Deposit into Account", deposit);
            Option oTransfer = new Option("Transfer between two accounts", transfer);
            Option oUpdate = new Option("Update Customer Details",
                new Option("Update Name", updateDetails, Fields.Name),
                new Option("Update Gender", updateDetails, Fields.Gender),
                new Option("Update Phone Nos", updateDetails, Fields.PhoneNos),
                new Option("Update Email Address", updateDetails, Fields.Email),
                new Option("Update Address", updateDetails, Fields.Address),
                new Option("Main menu", MainMenu));
            Option oDirectory = new Option("Bank Directory",
                new Option("Get Customer Details", getCustomerDetails),
                new Option("Get Customer Account Details", getAccountDetails),
                new Option("Get Customer Transaction History", getTransactionHistory),
                new Option("Main menu", MainMenu));
            showOptions(new[] { oCreate, oWithdraw, oDeposit, oTransfer, oUpdate, oDirectory});
            return true;
        }

        private bool updateDetails(object[] args)
        {
            string acc = input("Account number", ACCOUNTNOSPREDICATE);
            if (accounts.Keys.Contains(acc))
            {
                switch ((Fields) args[0])
                {
                    case Fields.Name:
                        var title = input("Enter Title", TEXTPREDICATE);
                        var fName = input("Enter First Name", TEXTPREDICATE);
                        var lName = input("Enter Last Name", TEXTPREDICATE);
                        accounts[acc].Customer.Title = title;
                        accounts[acc].Customer.FName = fName;
                        accounts[acc].Customer.LName = lName;
                        Console.WriteLine("Updates");
                        save();
                        return true;
                    case Fields.Gender:
                        var gender = Gender.Female;
                        if (String.Compare(input("Enter Gender", GENDERPREDICATE), "male", true)==0) gender = Gender.Male;
                        accounts[acc].Customer.Gender = gender;
                        Console.WriteLine("Updates");
                        save();
                        return true;
                    case Fields.Address:
                        var address = input("Enter Address", null);
                        accounts[acc].Customer.Address = address;
                        Console.WriteLine("Updates");
                        save();
                        return true;
                    case Fields.PhoneNos:
                        save();
                        break;
                    case Fields.Email:
                        save();
                        break;

                }
            }
            Console.WriteLine("Account not found!");
            return false;
        }

        private bool getTransactionHistory(params Object[] args)
        {
            string acc = input("Account number", ACCOUNTNOSPREDICATE);
            if (accounts.Keys.Contains(acc))
            {
                int c = 1;
                foreach (Transaction t in accounts[acc].GetTransactions()) {
                    Console.WriteLine("Transaction {0}\n", c);
                    Console.WriteLine(t.ToString());
                    Console.WriteLine();
                    c += 1;
                }
                Console.WriteLine();
                return true;
            }
            Console.WriteLine("Does not Exist\n");
            return true;
            
        }

        private bool getAccountDetails(params Object[] args)
        {
            string acc = input("Account number", ACCOUNTNOSPREDICATE);
            if (accounts.Keys.Contains(acc))
            {
                Console.WriteLine(accounts[acc].ToString());
                return true;
            }
            Console.WriteLine("Does not Exist\n");
            return false;
        }

        private bool getCustomerDetails(params Object[] args)
        {
            string name = input("Name or Account number", null).ToUpper();
            if (customers.Keys.Contains(name))
            {
                Console.WriteLine(customers[name].ToString());
                return true;
            }
            if (accounts.Keys.Contains(name))
            {
                Console.WriteLine(accounts[name].Customer.ToString());
                return true;
            }
            Console.WriteLine("Does not Exist\n");
            return false;
        }
        string input(string request, Action predicate, params Object[] args)
        {
            Console.Write("{0}?: ", request);
            String answer = Console.ReadLine();
            if (answer.Length == 0 || predicate != null && !predicate(answer, args))
            {
                Console.Write("Wrong input! Please try again \n");
                return input(request, predicate, args);
            }
            return answer;
        }
        void showOptions(params Option[] options)
        {
            Clear();
            for (int i = 0; i < options.Length; i++)
            {
                Option option = options[i];
                Console.WriteLine(i + 1 + ". " + option.Message);
            }
            Console.WriteLine();
            Console.Write("Please Take an Action(Enter number in range 1-{0} inclusive): ", options.Length);
            int action = -1;
            string inp = Console.ReadLine();
            Console.WriteLine();
            if (inp.Equals("break")) return;
            int.TryParse(inp, out action);

            if (action < 1 || action > options.Length)
            {
                Console.WriteLine("You entered an invalid number, try again\n");
                Console.Write(">Hit a key to CONTINUE<");
                Console.CursorVisible = false;
                Console.ReadKey();
                Console.CursorVisible = true;
                showOptions(options);
                return;
            }
            if (options[action - 1].Callback == null)
            {
                Option[] subOptions = options[action - 1].SubOptions;
                showOptions(subOptions);   
                return;
            }
            options[action - 1].Callback(options[action - 1].Args);

        }

        private bool transfer(params object[] args)
        {
            string fromNumber = input("Enter sender's account number", value => value[0].ToString().Length == 9);
            string toNumber = input("Enter beneficiary's account number", value => value[0].ToString().Length == 9);
            double amount = 0;
            input("Enter amount", value => double.TryParse(value[0].ToString(), out amount));
            if (!accounts.Keys.Contains(fromNumber) ){
                Console.WriteLine("Inavlid Sender's account number\n");
                return false;
            }
            if (!accounts.Keys.Contains(toNumber))
            {
                Console.WriteLine("Invalid Reciever's account number\n");
                return false;
            }
            Transaction transaction = new Transaction(amount, ETransactionType.Transfer, accounts[fromNumber], accounts[toNumber]);
            transaction.transact();
            transactions.Add(transaction);
            if (transaction.Result == ETransactionResult.Success)
            {
                save();

            }
            Console.WriteLine("TRANSACTION RECIEPT\n{0}", transaction);
            return true;
        }

        private bool withdraw(params object[] args)
        {
            string number = input("Enter account number", ACCOUNTNOSPREDICATE);
            double amount = double.Parse(input("Enter amount", FUNDSPREDICATE));
            if (accounts.Keys.Contains(number))
            {
                Transaction transaction = new Transaction(amount, ETransactionType.Withdrawal, accounts[number]);
                transaction.transact();
                transactions.Add(transaction);
                if (transaction.Result == ETransactionResult.Success)
                {
                    save();

                }
                Console.WriteLine("TRANSACTION RECIEPT\n{0}", transaction);
                return true;
            }
            Console.WriteLine("Invalid Account number");
            return false;
        }

        private bool deposit(params object[] args)
        {
            string number = input("Enter account number", ACCOUNTNOSPREDICATE);
            double amount = double.Parse(input("Enter amount", FUNDSPREDICATE));
            if (accounts.Keys.Contains(number))
            {
                Transaction transaction = new Transaction(amount, ETransactionType.Deposit, accounts[number]);
                transaction.transact();
                transactions.Add(transaction);
                if (transaction.Result == ETransactionResult.Success)
                {
                    save();

                }
                Console.WriteLine("TRANSACTION RECIEPT\n{0}", transaction);
                return true;
            }
            Console.WriteLine("Invalid Account number");
            return false;
        }

        private Customer createCustomer()
        {

            var title = input("Enter Title", TEXTPREDICATE);
            var fName = input("Enter First Name", TEXTPREDICATE);
            var lName = input("Enter Last Name", TEXTPREDICATE);

            var genderInp = input("Enter Gender", GENDERPREDICATE);
            var gender = Gender.Female;
            if (String.Compare(genderInp, "male", true) == 0)
                gender = Gender.Male;
            var address = input("Enter Address", null);
            Customer cust = new Customer(title, fName, lName, gender, address, DateTime.Now);
            return cust;
        }

        private bool exisitingUserAccount(params object[] args)
        {
            createAccount(input("Account nos", ACCOUNTNOSPREDICATE), args[0]);
            return true;
        }

        private bool createAccount(params object[] args)
        {
            Customer cust;
            if (args.Length > 1)
            {
                String nos = args[1].ToString();
                if (accounts.Keys.Contains(nos))
                {
                    cust = accounts[nos].Customer;
                }
                Console.WriteLine("Account does not exist! ");
                return false;
            }
            else
            {
                cust = createCustomer();
            }
            switch ((Fields)args[0])
            {
                case Fields.Savings:
                    var initialDeposit = double.Parse(input("Enter an initial deposit", FUNDSPREDICATE));
                    try
                    {
                        SavingsAccount savingsAcc = new SavingsAccount(cust, initialDeposit);
                        cust.addAccount(savingsAcc);
                        accounts.Add(savingsAcc.AccountNos, savingsAcc);
                        String name = (cust.LName + " " + cust.FName).ToUpper();
                        if (!customers.Keys.Contains(name)) customers.Add(name, cust);
                        Console.WriteLine(savingsAcc.AccountNos);
                        Console.WriteLine("Account Created Successfully! \n");
                        Console.WriteLine(cust);
                        Console.WriteLine("-----Account Details-----\n");
                        Console.WriteLine(savingsAcc);
                        save();
                        return true;
                    }
                    catch (ArgumentException E)
                    {
                        Console.WriteLine(E.Message);
                    }
                    break;
                case Fields.Current:
                    initialDeposit = double.Parse(input("Enter an initial deposit", FUNDSPREDICATE));
                    try {
                        CurrentAccount currentAcc = new CurrentAccount(cust, initialDeposit);
                        cust.addAccount(currentAcc);
                        accounts.Add(currentAcc.AccountNos, currentAcc);
                        String name = (cust.LName + " " + cust.FName).ToUpper();
                        if (!customers.Keys.Contains(name)) customers.Add(name, cust);
                        Console.WriteLine(currentAcc.AccountNos);
                        Console.WriteLine("Account Created Successfully! \n");
                        Console.WriteLine(cust);
                        Console.WriteLine("-----Account Details-----\n");
                        Console.WriteLine(currentAcc);
                    }
                    catch (ArgumentException E)
                    {
                        Console.WriteLine(E.Message);
                    }
                    save();
                    return true;
                case Fields.Deposit:
                    save();
                    return true;
                default:
                    return false;

            }
            return false;
        }

        public void Clear()
        {
            Console.Clear();
            Console.WriteLine("Welcome to NACOSS Bank inc. interface \n");;
        }

        public void save()
        {
            Directory.CreateDirectory("account");
            var serializer = new BinaryFormatter();
            foreach(var item in accounts)
            {
                var stream = new FileStream("account/"+item.Key+".acc", FileMode.OpenOrCreate, FileAccess.Write);
                serializer.Serialize(stream, item.Value);
                stream.Close();

            }
        }

        Dictionary<string, Account> getAccounts()
        {
            var accounts = new Dictionary<string, Account>();
            var serializer = new BinaryFormatter();
            foreach (var file in Directory.GetFiles("account"))
            {
                var stream = new FileStream(file, FileMode.Open);
                stream.Seek(0, SeekOrigin.Begin);
                Account acc = (Account) serializer.Deserialize(stream);
                accounts.Add(acc.AccountNos, acc);
                stream.Close();
            }
            Account.NosAccount = accounts.Count;
            return accounts;
        }
    }

    class Option
    {
        public String Message { get; }
        public Action Callback { get; }
        Option[] subOptions;
        object[] args;
        public Option[] SubOptions
        {
            get
            {

                Option[] ret = new Option[subOptions.Length];
                for (int i = 0; i < subOptions.Length; i++)
                {
                    ret[i] = subOptions[i];
                }
                return ret;
            }
        }
        public object[] Args
        {
            get
            {
                object[] ret = new object[args.Length];
                for(int i=0; i<args.Length; i++)
                {
                    ret[i] = args[i];
                }
                return ret;
            }
        }
        public Option(String message, Action callback, params object[] args)
        {
            this.Callback = callback;
            this.Message = message;
            this.args = new object[args.Length];
            Array.Copy(args, this.args, args.Length);
        }
        public Option(String message, params Option[] subOptions)
        {
            this.Message = message;
            this.subOptions = new Option[subOptions.Length];
            Array.Copy(subOptions, this.subOptions, subOptions.Length);
        }

    }
}
