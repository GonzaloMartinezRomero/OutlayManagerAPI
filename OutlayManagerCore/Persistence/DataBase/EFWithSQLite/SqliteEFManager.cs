using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using OutlayManagerCore.Model.OutlayResume;
using OutlayManagerCore.Model.Transaction;
using OutlayManagerCore.Persistence.DataBase.EFWithSQLite.Utilities;
using EntityModel = OutlayManagerCore.Persistence.DataBase.EFWithSQLite.EntityModel;

namespace OutlayManagerCore.Persistence.DataBase.EFWithSQLite
{
    internal class SqliteEFManager : IPersistenceServices
    {
        private const string DATE_FORMAT = "dd/MM/yyyy";
        private SQLiteContext contextDB;

        private SqliteEFManager() { }

        public SqliteEFManager(string pathDataSource)
        {
            contextDB = new SQLiteContext(pathDataSource);  
        }

        #region Transactions
        public void SaveTransaction(Model.Transaction.Transaction transaction)
        {
            var newTransaction = new EntityModel.Transaction()
            {
                Amount = transaction.Amount,
                Date = transaction.Date.ToString(DATE_FORMAT),
                TransactionDetail_FK = new EntityModel.DetailTransaction()
                {
                    Code = transaction?.DetailTransaction.Code,
                    Description = transaction?.DetailTransaction.Description,
                    TypeTransaction = transaction?.DetailTransaction.Type.ToString()
                }
            };

            contextDB.Transaction.Add(newTransaction);
            contextDB.SaveChanges();

            transaction.ID = (uint)newTransaction.ID;
        }

        public void UpdateTransaction(Transaction transaction)
        {
            IEnumerable<EntityModel.Transaction> transactionsBD = from EntityModel.Transaction t
                                                                  in contextDB.Transaction
                                                                              .Include(x => x.TransactionDetail_FK)
                                                                  where t.ID == transaction.ID
                                                                  select t;

            EntityModel.Transaction transactionToUpdate = transactionsBD.SingleOrDefault();

            if (transactionToUpdate != null)
            {
                transactionToUpdate.Amount = transaction.Amount;
                transactionToUpdate.Date = transaction.Date.ToString(DATE_FORMAT);
                transactionToUpdate.TransactionDetail_FK.Code = transaction.DetailTransaction.Code;
                transactionToUpdate.TransactionDetail_FK.Description = transaction.DetailTransaction.Description;
                transactionToUpdate.TransactionDetail_FK.TypeTransaction = transaction.DetailTransaction.Type.ToString();

                contextDB.SaveChanges();
            }
        }


        public Model.Transaction.Transaction GetTransaction(uint transactionID)
        {
            IEnumerable<Model.Transaction.Transaction> transactionsBD = from EntityModel.Transaction t
                                                                        in contextDB.Transaction
                                                                                    .Include(x => x.TransactionDetail_FK)
                                                                                     .AsNoTracking()
                                                                        where t.ID == transactionID
                                                                        select Casting.ToModelTransaction(t);

            return transactionsBD.SingleOrDefault() ?? new Model.Transaction.Transaction();
        }

        public IEnumerable<Model.Transaction.Transaction> GetTransactions(int year, int month)
        {
            var transactionsBD = (from EntityModel.Transaction t
                                  in contextDB.Transaction
                                              .Include(x => x.TransactionDetail_FK)
                                              .AsNoTracking()
                                  select t)
                                  .AsEnumerable()
                                  .Where(t => t.Date.ToDateTime().Year == year && t.Date.ToDateTime().Month == month)
                                  .Select(x => Casting.ToModelTransaction(x));

            return transactionsBD;
        }

        public IEnumerable<Model.Transaction.Transaction> GetAllTransactions()
        {
            var transactionsBD = (from EntityModel.Transaction t
                                   in contextDB.Transaction
                                               .Include(x => x.TransactionDetail_FK)
                                               .AsNoTracking()
                                  select t)
                                   .AsEnumerable()
                                   .Select(x => Casting.ToModelTransaction(x));

            return transactionsBD;
        }

        public void DeleteTransaction(uint transactionID)
        {
            IEnumerable<EntityModel.Transaction> transactionsBD = from EntityModel.Transaction t
                                                                  in contextDB.Transaction
                                                                              .Include(x => x.TransactionDetail_FK)
                                                                  where t.ID == transactionID
                                                                  select t;

            EntityModel.Transaction transactionToDelete = transactionsBD.SingleOrDefault();

            if (transactionToDelete != null)
            {
                contextDB.Transaction.Remove(transactionToDelete);
                contextDB.SaveChanges();
            }
        }
        #endregion

        #region OutalayResume
        public IEnumerable<OutlayResume> OutlayResume()
        {
            throw new NotImplementedException();
        }

        public OutlayResume OutlayResume(int year, int month)
        {
            throw new NotImplementedException();
        }

        public void SaveConsolidateMonth(OutlayResume outlayResume)
        {
            throw new NotImplementedException();
        }

        #endregion

        public void Dispose()
        {
            this.contextDB.Dispose();
        }

      
    }
}
