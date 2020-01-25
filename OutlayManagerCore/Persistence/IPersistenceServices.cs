using OutlayManagerCore.Model.OutlayResume;
using OutlayManagerCore.Model.Transaction;
using System;
using System.Collections.Generic;
using System.Text;

namespace OutlayManagerCore.Persistence
{
    internal interface IPersistenceServices : IDisposable
    {
        void SaveTransaction(Transaction transaction);
        void DeleteTransaction(uint transactionID);
        void UpdateTransaction(Transaction transaction);
        IEnumerable<Transaction> GetAllTransactions();
        IEnumerable<Transaction> GetTransactions(int year, int month);
        Transaction GetTransaction(uint transacionID);
        OutlayResume OutlayResume(int year, int month);
        IEnumerable<OutlayResume> OutlayResume();
        void SaveConsolidateMonth(OutlayResume outlayResume);
    }
}
