using Microsoft.EntityFrameworkCore;
using OutlayManagerCore.Persistence.DataBase.EFWithSQLite.EntityModel;

namespace OutlayManagerCore.Persistence.DataBase.EFWithSQLite
{
    public class SQLiteContext : DbContext
    {
        public DbSet<TransactionOutlay> TransactionOutlay { get; set; }

        public DbSet<MCodeTransaction> MCodeTransaction { get; set; }

        public DbSet<MTypeTransaction> MTypeTransaction { get; set; }

        public SQLiteContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }
        
    }
}
