using Microsoft.EntityFrameworkCore;
using OutlayManagerCore.Persistence.DataBase.EFWithSQLite.EntityModel;
using System;

namespace OutlayManagerCore.Persistence.DataBase.EFWithSQLite
{
    public class SQLiteContext : DbContext
    {
        private readonly string pathDataSource;

        public DbSet<Transaction> Transaction { get; set; }

        public DbSet<MCodeTransaction> MCodeTransactions { get; set; }

        public DbSet<MTypeTransaction> MTypeTransactions { get; set; }

        private SQLiteContext() { }

        public SQLiteContext(string pathDataSource)
        {
            this.pathDataSource = "Data Source=" + pathDataSource ?? throw new ArgumentNullException("pathDataSource cant be null");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //MIRAR EL TEMA DE EF MIGRATIONS!!
            optionsBuilder.UseSqlite(pathDataSource);
        }
    }
}
