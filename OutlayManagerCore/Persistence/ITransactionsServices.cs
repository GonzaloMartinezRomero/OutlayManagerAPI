using OutlayManagerCore.Model.Transaction;
using System;
using System.Collections.Generic;

namespace OutlayManagerCore.Persistence
{
    public interface ITransactionsServices : IDisposable
    {
        void SaveTransaction(TransactionDTO transaction);
        void DeleteTransaction(uint transactionID);
        void UpdateTransaction(TransactionDTO transaction);
        List<TransactionDTO> GetAllTransactions();
        List<TransactionDTO> GetTransactions(int year, int month);
        TransactionDTO GetTransaction(uint transacionID);
        List<CodeTransactionDTO> GetMCodeTransactions();
        List<TypeTransactionDTO> GetMTypeTransactions();
    }
}
