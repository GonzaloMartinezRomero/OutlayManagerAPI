using OutlayManagerAPI.Model.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OutlayManager.ExternalService.TransactionBus.Abstract
{
    public interface ITransactionServiceBus
    {
        Task<IEnumerable<TransactionDTO>> SyncExternalTransactionAsync();

    }
}
