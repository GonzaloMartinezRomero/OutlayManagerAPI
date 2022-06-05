using OutlayManagerAPI.Model.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OutlayManager.Infraestructure.Services.Abstract
{
    public interface ITransactionSync : IDisposable
    {
        Task<IEnumerable<TransactionDTO>> SyncExternalTransactionAsync();
        
    }
}
