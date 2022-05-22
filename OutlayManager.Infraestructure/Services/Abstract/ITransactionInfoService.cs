using OutlayManagerAPI.Model.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OutlayManagerAPI.Infraestructure.Services.Abstract
{
    public interface ITransactionInfoService
    {  
        Task<List<TypeTransactionDTO>> TransactionsTypes();
        Task<List<int>> YearsAvailabes();
        Task<List<AmountResume>> AmountResumes();
    }
}
