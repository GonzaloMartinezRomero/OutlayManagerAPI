using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OutlayManagerCore.Model.OutlayResume;
using OutlayManagerCore.Model.Transaction;

namespace OutlayManagerCore.Persistence.MemoryData
{
    internal class MemoryDataPersistence : IPersistenceServices
    {
        private List<Transaction> transactions = new List<Transaction>()
        {
            new Transaction()
            {
                ID = 1,
                Amount = 10.0d,
                Date = new DateTime(2019,09,01),
                DetailTransaction = new TransactionDetail()
                {
                    Code = "Mercadona",
                    Description = "Compra de mierda",
                    Type = TransactionDetail.TypeTransaction.SPENDING
                }
            },
            new Transaction()
            {
                ID = 2,
                Amount = 25.3d,
                Date = new DateTime(2019,09,02),
                DetailTransaction = new TransactionDetail()
                {
                    Code = "Salir",
                    Description = "Cervezas",
                    Type = TransactionDetail.TypeTransaction.SPENDING
                }
            },
            new Transaction()
            {
                ID = 3,
                Amount = 10.0d,
                Date = new DateTime(2019,09,05),
                DetailTransaction = new TransactionDetail()
                {
                    Code = "Nomina normal",
                    Description = "Nomina de mierda",
                    Type = TransactionDetail.TypeTransaction.INCOMING
                }
            }
        };

        public void DeleteTransaction(uint transactionID)
        {
            transactions.Remove(transactions.Find(x => x.ID == transactionID));
        }

        public IEnumerable<OutlayResume> GetOutlayResumes()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Transaction> GetTransactions(int year, int month)
        {
            return transactions.Where(x => x.Date.Year == year && x.Date.Month == month);
        }

        public Transaction GetTransaction(uint transacionID)
        {
            return transactions.Find(x => x.ID == transacionID);
        }

        public void SaveTransaction(Transaction transaction)
        {
            if (!transactions.Any(x => x.ID == transaction.ID))
            {
                transactions.Add(transaction);
            }
            else
            {
                //En el caso de existir se podria actualizar
                throw new Exception($"Transaction with ID={transaction.ID} already exists!");
            }
        }

        public void Dispose()
        {
            
        }

        public void UpdateTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public OutlayResume OutlayResume(int year, int month)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OutlayResume> OutlayResume()
        {
            throw new NotImplementedException();
        }

        public void SaveConsolidateMonth(OutlayResume outlayResume)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Transaction> GetAllTransactions()
        {
            throw new NotImplementedException();
        }
    }
}
