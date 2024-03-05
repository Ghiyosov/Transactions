using Dapper;
using Domein.Models;
using Infrastruction.DataDapper;

namespace Infrastruction.Services;

public class AccountService
{
    readonly DapperContext _context;
    public AccountService()
    {
        _context = new DapperContext();
    }

    public List<Accounts> GetAccounts()
    {
        var sql = _context.Connection().Query<Accounts>("select * from accounts").ToList();
        return sql;
    }
    public void AddAccount(Accounts account)
    {
        _context.Connection().Execute("insert into accounts(fullname,balance)values(@fullname,@balance)", account);
    }
    public void UpdateAccoount(Accounts account)
    {
        _context.Connection().Execute("update accounts set fullname=@fullname where id=@id", new { FullName = account.FullName, Id = account.Id });
    }
    public void AddBalace(Accounts account)
    {
        var sql =_context.Connection().QuerySingleOrDefault<Accounts>("select * from accouts where id=@id",new{Id=account.Id});
        _context.Connection().Execute("update accouts set balance=@balance where id=@id",new{Balance=(sql.Balance+account.Balance),Id=account.Id});
    }
}
