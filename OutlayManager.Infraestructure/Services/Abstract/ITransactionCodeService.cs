using OutlayManagerAPI.Model.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OutlayManagerAPI.Infraestructure.Services.Abstract
{
    public interface ITransactionCodeService
    {
        Task<int> AddTransactionCode(CodeTransactionDTO codeTransactionDTO);

        Task<int> DeleteTransactionCode(uint id);

        Task<List<CodeTransactionDTO>> TransactionsCodes();
    }
}
