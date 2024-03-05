using Dapper;
using Domein.Models;
using Infrastruction.DataDapper;

namespace Infrastruction.Services;

public class TransactionServices
{
    readonly DapperContext _context;
    public TransactionServices()
    {
        _context = new DapperContext();
    }
    public List<Story> GetStoriesOfTransactions() //istoriay transactions meta
    {
        var sql = _context.Connection().Query<Story>(@"select ac.fullname as name_sender,acc.fullname as Name_Recipient, t.sum_transactions as sum_transactions, t.date_transaction as Date_Transaction  
                                                        from transactions as t
                                                        join accounts as ac on t.account_sender=ac.id
                                                        join accounts as acc on t.accout_recipient=acc.id").ToList();
        return sql;
    }
    public StoryTransactions SenderStory(int id) //istoriay sendi accounta meta
    {
        var sql = @"select fullname as name_sender  
                    from accounts as ac
                    where ac.id=@id;
                    select 'recipienter:'||ac.fullname ||' date:'|| t.Date_Transaction ||' sum:'||t.Sum_Transactions as name_recipients
                    from transactions as t
                    join accounts as ac on t.Account_Sender=@id
                    join accounts as acc on t.Accout_Recipient=acc.id;";
        using (var multiple = _context.Connection().QueryMultiple(sql, new { Id = id }))
        {
            var recipiendStory = new StoryTransactions();
            recipiendStory.Name_Sender = multiple.ReadFirst<string>();
            recipiendStory.Name_Recipients = multiple.Read<string>().ToList();
            return recipiendStory;
        }
    }
    public StoryTransactions RecipientStory(int id) //istoriay recipiend accounta meta
    {
        var sql = @"select fullname as name_sender
                    from accounts as ac
                    where ac.id=@id;
                    select 'sender:'||acc.fullname ||' date:'|| t.Date_Transaction ||' sum:'||t.Sum_Transactions as name_recipients
                    from transactions as t
                    join accounts as ac on t.Account_Sender=ac.id
                    join accounts as acc on t.Accout_Recipient=@id;";
        using (var multiple = _context.Connection().QueryMultiple(sql, new { Id = id }))
        {
            var recipiendStory = new StoryTransactions();
            recipiendStory.Name_Sender = multiple.ReadFirst<string>();
            recipiendStory.Name_Recipients = multiple.Read<string>().ToList();
            return recipiendStory;
        }

    }
    public void AddTransaction(Transactions transactions)
    {
        transactions.Date_Transaction= Convert.ToString(DateTime.Now);
        Accounts a1 = _context.Connection().QuerySingleOrDefault<Accounts>("select * from accounts where id=@id", new { Id = transactions.Account_Sender });
        Accounts a2 = _context.Connection().QuerySingleOrDefault<Accounts>("select * from accounts where id=@id", new { Id = transactions.Accout_Recipient });

        _context.Connection().Execute("update accounts set balance=@balance where id=@id", new { Balance = (a1.Balance - transactions.Sum_Transactions), Id = transactions.Account_Sender });
        _context.Connection().Execute("update accounts set balance=@balance where id=@id", new { Balance = (a2.Balance + transactions.Sum_Transactions), Id = transactions.Accout_Recipient });
        _context.Connection().Execute(@"insert into transactions(Account_Sender,Accout_Recipient,Sum_Transactions,Date_Transaction)
                                        values(@Account_Sender,@Accout_Recipient,@Sum_Transactions,@Date_Transaction)",transactions);
    }
}

