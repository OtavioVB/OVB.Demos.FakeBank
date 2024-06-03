using Microsoft.EntityFrameworkCore;

namespace OVB.Demos.FakeBank.Account.Infrascructure.EntityFrameworkCore;

public sealed class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }
}
