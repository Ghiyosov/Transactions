using System.Data.Common;
using Domein.Models;
using Infrastruction.Services;

var account = new AccountService();
var transaction = new TransactionServices();

while (true)
{
    System.Console.WriteLine("ad - add account");
    System.Console.WriteLine("t - do transactions");
    System.Console.WriteLine("st - story of send");
    System.Console.WriteLine("rt - story of recipient");
    System.Console.WriteLine("ts - story of all transactions");

    string command = Console.ReadLine();
    command = command.ToLower();
    if (command == "ad")
    {
        System.Console.Write("FullName : ");
        string name = Console.ReadLine();
        System.Console.Write("Balance : ");
        decimal balance = Convert.ToDecimal(Console.ReadLine());
        var acc = new Accounts();
        acc.FullName = name;
        acc.Balance = balance;
        account.AddAccount(acc);
    }
    else if (command == "t")
    {
        var trans = new Transactions();
        System.Console.Write("Account_Sender : ");
        trans.Account_Sender = Convert.ToInt32(Console.ReadLine());
        System.Console.Write("Account_Recipient : ");
        trans.Accout_Recipient = Convert.ToInt32(Console.ReadLine());
        System.Console.Write("Sum : ");
        trans.Sum_Transactions = Convert.ToDecimal(Console.ReadLine());
        transaction.AddTransaction(trans);
    }
    else if (command=="st")
    {
        System.Console.Write("Account Id : ");
        int id = Convert.ToInt32(Console.ReadLine());
        var sst = transaction.SenderStory(id);
        System.Console.WriteLine($"{sst.Name_Sender}");
        System.Console.WriteLine("*********************************");
        foreach (var item in sst.Name_Recipients)
        {
            System.Console.WriteLine(item);
            System.Console.WriteLine("-----------------------------");
        }
    }
    else if (command=="rt")
    {
        System.Console.Write("Account Id : ");
        int id = Convert.ToInt32(Console.ReadLine());
        var sst = transaction.RecipientStory(id);
        System.Console.WriteLine($"{sst.Name_Sender}");
        System.Console.WriteLine("*********************************");
        foreach (var item in sst.Name_Recipients)
        {
            System.Console.WriteLine(item);
            System.Console.WriteLine("-----------------------------");
        }
    }
    else if (command=="ts")
    {
        List<Story> dd = transaction.GetStoriesOfTransactions();
        foreach (var item in dd)
        {
            System.Console.WriteLine($"{item.Name_Sender} sender : {item.Sum_Transactions} to {item.Name_Recipient} at : {item.Date_Transaction}");
            System.Console.WriteLine("----------------------------------------------------------");
        }
    }
}