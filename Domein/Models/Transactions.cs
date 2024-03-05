namespace Domein.Models;

public class Transactions
{
    public int Id { get; set; }
    public int Account_Sender { get; set; }
    public int Accout_Recipient { get; set; }
    public decimal Sum_Transactions { get; set; }
    public string Date_Transaction { get; set; }
}
