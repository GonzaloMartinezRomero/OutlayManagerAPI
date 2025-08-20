using OutlayManager.Model.DTO;
using OutlayManagerAPI.Model.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OutlayManagerAPI.Infraestructure.Services.Abstract
{
    public interface ITransactionInfoService
    {  
        public Task<List<TypeTransactionDTO>> GetTransactionsTypesAsync();

        public Task<List<int>> YearsAvailabes();

        public Task<List<AmountResume>> AmountResumes();

        public Task<List<SavingPerYearDto>> SavingsPerYearAsync();
        
        public Task<List<TransactionResume>> ResumeByTransactionAsync(int year);
    }
}
