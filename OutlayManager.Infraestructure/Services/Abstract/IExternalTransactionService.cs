using OutlayManagerAPI.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlayManager.Infraestructure.Services.Abstract
{
    public interface IExternalTransactionService : IDisposable
    {
        Task<IEnumerable<TransactionDTO>> DownloadExternalTransaction();
    }
}
