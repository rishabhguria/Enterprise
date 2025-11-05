using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

public class SMDbContext : DbContext
{
    public SMDbContext() : base("name=SMDbContext")
    {
    }

    public DbSet<T_SMSymbolLookUpTable> T_SMSymbolLookUpTable { get; set; }
}

public class T_SMSymbolLookUpTable
{
    [Key]
    public long Symbol_PK { get; set; }

    public string TickerSymbol { get; set; }

    public string CUSIPSymbol { get; set; }
}
