using Microsoft.EntityFrameworkCore;
using OVB.Demos.FakeBank.Account.Domain.AccountContext.DataModels;
using OVB.Demos.FakeBank.Account.Infrascructure.EntityFrameworkCore.Mappings;

namespace OVB.Demos.FakeBank.Account.Infrascructure.EntityFrameworkCore;

public sealed class DataContext : DbContext
{
    public DbSet<AccountDataModel> Accounts { get; set; }

    public DataContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AccountMapping());
        base.OnModelCreating(modelBuilder);
    }
}
