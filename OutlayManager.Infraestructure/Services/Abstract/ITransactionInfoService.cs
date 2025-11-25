using OutlayManager.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OutlayManagerAPI.Infraestructure.Services.Abstract
{
    public interface ITransactionInfoService
    {  
        public Task<List<TypeTransactionDTO>> GetTransactionsTypesAsync();

        public Task<List<int>> YearsAvailabes();

        public Task<List<AmountResume>> AmountResumes();

        public Task<List<PayrollPerYearDto>> PayrollPerYearAsync();
        
        public Task<List<TransactionResume>> ResumeByTransactionAsync(int year);
        
        public Task<MonthResume> MonthResumeAsync(int year, int month);

        public Task<TotalAmount> TotalAmountAsync();
    }
}
