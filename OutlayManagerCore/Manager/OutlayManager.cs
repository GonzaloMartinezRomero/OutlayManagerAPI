using OutlayManagerCore.Model.OutlayResume;
using OutlayManagerCore.Model.ResultInfo;
using OutlayManagerCore.Model.Transaction;
using OutlayManagerCore.Persistence;
using OutlayManagerCore.Persistence.MemoryData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OutlayManagerCore.Manager.OutlayManager
{
    internal class OutlayManager 
    {
        IPersistenceServices persistenceService;

        public OutlayManager(IPersistenceServices persistenceService)
        {
            this.persistenceService = persistenceService ?? throw new ArgumentNullException($"{this.GetType().Name}: Persistence service is null");            
        }

        public StateInfo AddTransaction(Transaction transaction)
        {
            try
            {
                persistenceService.SaveTransaction(transaction);
                return new StateInfo("Transaction saved!", transaction.ID);

            }catch(Exception e)
            {
                return new StateInfo(e);
            }
        }

        public IEnumerable<Transaction> GetTransactions(int year, int month)
        {
            return persistenceService.GetTransactions(year, month);   
        }

        public Transaction GetTransaction(uint transactionID)
        {
            return persistenceService.GetTransaction(transactionID) ?? throw new Exception("Transaction not found");
        }

        public StateInfo DeleteTransaction(uint transactionID)
        {
            try
            {
                persistenceService.DeleteTransaction(transactionID);
                return new StateInfo("Transaction deleted", transactionID);

            }catch(Exception e)
            {
                return new StateInfo(e);
            }
        }

        public void ConsolidateMonth(int year, int month)
        {
            List<Transaction> transactions = persistenceService.GetTransactions(year, month)
                                                               .ToList();

            double spenses = transactions.Where(x => x.DetailTransaction.Type == TransactionDetail.TypeTransaction.SPENDING)
                                         .Select(x => x.Amount)
                                         .Sum();

            double incoming = transactions.Where(x => x.DetailTransaction.Type == TransactionDetail.TypeTransaction.INCOMING)
                                         .Select(x => x.Amount)
                                         .Sum();

            double adjust = transactions.Where(x => x.DetailTransaction.Type == TransactionDetail.TypeTransaction.ADJUST)
                                         .Select(x => x.Amount)
                                         .Sum();

            OutlayResume outlayResume= persistenceService.OutlayResume(year, month);

            if(outlayResume == null)
            {
                persistenceService.SaveConsolidateMonth(new OutlayResume()
                {
                    Year = year,
                    Month = month,
                    Income = incoming,
                    Expense = spenses + adjust
                });
            }
            else
            {
                outlayResume.Expense = spenses + adjust;
                outlayResume.Income = incoming;

                persistenceService.SaveConsolidateMonth(outlayResume);
            }
        }

        internal IEnumerable<Transaction> GetAllTransactions()
        {
            return persistenceService.GetAllTransactions() ?? throw new Exception("Transaction not found");
        }

        internal StateInfo UpdateTransaction(Transaction transaction)
        {
            try
            {
                persistenceService.UpdateTransaction(transaction);
                return new StateInfo("Transaction updated", transaction.ID);

            }
            catch (Exception e)
            {
                return new StateInfo(e);
            }
        }
    }
}
