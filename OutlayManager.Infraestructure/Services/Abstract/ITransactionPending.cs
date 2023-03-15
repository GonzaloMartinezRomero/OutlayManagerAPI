using OutlayManager.Model.DTO;
using OutlayManagerAPI.Model.DTO;
using System.Collections.Generic;

namespace OutlayManager.Infraestructure.Services.Abstract
{
    public interface ITransactionPending
    {
        public IEnumerable<TransactionDTO> SavePendingTransactions();        
    }
}
