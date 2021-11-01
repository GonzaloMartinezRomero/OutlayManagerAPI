using OutlayManagerAPI.Model;
using System.Collections.Generic;

namespace OutlayManagerAPI.Services.TransactionServices
{
    public interface ITransactionServices
    {
        void SaveTransaction(TransactionDTO transaction);
        void DeleteTransaction(uint transactionID);
        void UpdateTransaction(TransactionDTO transaction);
        List<TransactionDTO> GetAllTransactions();
        List<TransactionDTO> GetTransactions(int year, int month);
        TransactionDTO GetTransaction(uint transacionID);
        List<CodeTransactionDTO> GetMCodeTransactions();
        List<TypeTransactionDTO> GetMTypeTransactions();
        void AddTransactionCode(CodeTransactionDTO codeTransactionDTO);
        void DeleteTransactionCode(uint id);
        List<int> GetYearsAvailabes();
    }
}
