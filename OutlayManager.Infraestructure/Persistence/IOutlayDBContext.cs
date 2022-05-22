using Microsoft.EntityFrameworkCore;
using OutlayManagerCore.Infraestructure.Persistence.Model;

namespace OutlayManagerAPI.Infraestructure.Persistence
{
    internal interface IOutlayDBContext 
    {
        public DbContext Context { get; }

        public DbSet<TransactionOutlay> TransactionOutlay { get; set; }

        public DbSet<MCodeTransaction> MCodeTransaction { get; set; }

        public DbSet<MTypeTransaction> MTypeTransaction { get; set; }
    }
}
