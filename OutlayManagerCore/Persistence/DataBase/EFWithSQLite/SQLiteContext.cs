using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using OutlayManagerCore.Persistence.DataBase.EFWithSQLite.EntityModel;

namespace OutlayManagerCore.Persistence.DataBase.EFWithSQLite
{
    public class SQLiteContext : DbContext
    {
        private string pathDataSource;

        public DbSet<Transaction> Transaction { get; set; }

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
