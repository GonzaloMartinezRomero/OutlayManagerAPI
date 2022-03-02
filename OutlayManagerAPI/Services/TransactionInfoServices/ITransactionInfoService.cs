using OutlayManagerAPI.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OutlayManagerAPI.Services.TransactionInfoServices
{
    public interface ITransactionInfoService
    {  
        Task<List<TypeTransactionDTO>> TransactionsTypes();
        Task<List<int>> YearsAvailabes();
        Task<List<AmountResume>> AmountResumes();
    }
}
