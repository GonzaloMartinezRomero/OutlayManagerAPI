using OutlayManager.Model;
using System.Collections.Generic;

namespace OutlayManager.Infraestructure.Services.Abstract
{
    public interface ITransactionPending
    {
        public IEnumerable<TransactionDTO> SavePendingTransactions();        
    }
}
