using Microsoft.EntityFrameworkCore;
using OutlayManagerCore.Persistence.Model;

namespace OutlayManagerAPI.Persistence
{
    public interface IOutlayDBContext 
    {
        public DbContext Context { get; }

        public DbSet<TransactionOutlay> TransactionOutlay { get; set; }

        public DbSet<MCodeTransaction> MCodeTransaction { get; set; }

        public DbSet<MTypeTransaction> MTypeTransaction { get; set; }
    }
}
