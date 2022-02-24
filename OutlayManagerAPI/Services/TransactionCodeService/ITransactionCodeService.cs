using OutlayManagerAPI.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OutlayManagerAPI.Services.TransactionCodeService
{
    public interface ITransactionCodeService
    {
        Task<int> AddTransactionCode(CodeTransactionDTO codeTransactionDTO);

        Task<int> DeleteTransactionCode(uint id);

        Task<List<CodeTransactionDTO>> TransactionsCodes();
    }
}
