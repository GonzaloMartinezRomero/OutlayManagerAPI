using Microsoft.EntityFrameworkCore;
using OutlayManagerCore.Model.Transaction;
using OutlayManagerCore.Persistence.DataBase.EFWithSQLite.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OutlayManagerCore.Persistence.DataBase.EFWithSQLite
{
    internal class SqliteEFManager : ITransactionsServices
    {
        private const string DATE_FORMAT = "dd/MM/yyyy";
        private readonly SQLiteContext contextDB;

        private SqliteEFManager() { }

        public SqliteEFManager(string pathDataSource)
        {
            contextDB = new SQLiteContext(pathDataSource);  
        }

        #region Transactions
        public void SaveTransaction(Model.Transaction.TransactionDTO transaction)
        {
            if (transaction.CodeTransactionID <= 0)
                throw new Exception($"[{nameof(SaveTransaction)}] {nameof(TransactionDTO.CodeTransactionID)} is mandatory");

            if (transaction.TypeTransactionID <= 0)
                throw new Exception($"[{nameof(SaveTransaction)}] {nameof(TransactionDTO.TypeTransactionID)} is mandatory");

            var newTransaction = new EntityModel.Transaction()
            {
                Amount = transaction.Amount,
                Date = transaction.Date.ToString(DATE_FORMAT),
                Description = transaction.Description,
                CodeTransaction_ID = transaction.CodeTransactionID,
                TypeTransaction_ID = transaction.TypeTransactionID
            };

            contextDB.Transaction.Add(newTransaction);
            contextDB.SaveChanges();

            transaction.ID = (uint)newTransaction.ID;
        }

        public void UpdateTransaction(TransactionDTO transaction)
        {
            EntityModel.Transaction transactionToUpdate = (from EntityModel.Transaction t
                                                                  in contextDB.Transaction
                                                                              .Include(x => x.TypeTransaction)
                                                                              .Include(x => x.CodeTransaction)
                                                                  where t.ID == transaction.ID
                                                                  select t).SingleOrDefault();

            if (transactionToUpdate == null)
                throw new Exception($"[{nameof(UpdateTransaction)}] Requested transaction not exist in DB. ID:{transaction.ID}");
           
            transactionToUpdate.Amount = transaction.Amount;
            transactionToUpdate.Date = transaction.Date.ToString(DATE_FORMAT);
            transactionToUpdate.CodeTransaction_ID = transaction.CodeTransactionID;
            transactionToUpdate.Description = transaction.Description;
            transactionToUpdate.TypeTransaction_ID = transaction.TypeTransactionID;

            contextDB.SaveChanges();            
        }

        public Model.Transaction.TransactionDTO GetTransaction(uint transactionID)
        {
            Model.Transaction.TransactionDTO transaction = (from EntityModel.Transaction t
                                                            in contextDB.Transaction
                                                                        .AsNoTracking()
                                                                        .Include(x => x.TypeTransaction)
                                                                        .Include(x => x.CodeTransaction)
                                                            where t.ID == transactionID
                                                            select t)
                                                            .SingleOrDefault()
                                                            .ToTransactionDTO();

            return transaction;
        }

        public List<Model.Transaction.TransactionDTO> GetTransactions(int year, int month)
        {
            List<TransactionDTO> transactionsBD = (from EntityModel.Transaction t
                                                   in contextDB.Transaction
                                                               .AsNoTracking()
                                                               .Include(x => x.TypeTransaction)
                                                               .Include(x => x.CodeTransaction)
                                                   select t)
                                                   .ToList()
                                                   .Where(t =>
                                                   {
                                                       DateTime dateParsed = t.Date.ToDateTime();
                                                       return dateParsed.Year == year && dateParsed.Month == month;
                                                   })
                                                   .Select(x => x.ToTransactionDTO())
                                                   .ToList();

            return transactionsBD;
        }

        public List<Model.Transaction.TransactionDTO> GetAllTransactions()
        {
            List<TransactionDTO> transactionsBD = (from EntityModel.Transaction t
                                                    in contextDB.Transaction
                                                                .AsNoTracking()
                                                                .Include(x => x.TypeTransaction)
                                                                .Include(x => x.CodeTransaction)
                                                   select t)
                                                    .ToList()
                                                    .Select(x => x.ToTransactionDTO())
                                                    .ToList();

            return transactionsBD;
        }

        public void DeleteTransaction(uint transactionID)
        {
            EntityModel.Transaction transactionToDelete = (from EntityModel.Transaction t
                                                          in contextDB.Transaction
                                                           where t.ID == transactionID
                                                           select t)
                                                          .SingleOrDefault() ?? throw new Exception($"[{nameof(DeleteTransaction)}]: Transaction ID:{transactionID} not found");
        
        
            contextDB.Transaction.Remove(transactionToDelete);
            contextDB.SaveChanges();        
        }
        #endregion      

        public void Dispose()
        {
            this.contextDB.Dispose();
        }

        public List<CodeTransactionDTO> GetMCodeTransactions()
        {
            List<CodeTransactionDTO> codesTransactionsBD = (from EntityModel.MCodeTransaction codes
                                                     in contextDB.MCodeTransactions
                                                   select codes)
                                                   .ToList()
                                                   .Select(x=>x.ToCodeTransactionDTO())
                                                   .ToList();
            return codesTransactionsBD;
        }

        public List<TypeTransactionDTO> GetMTypeTransactions()
        {
            List<TypeTransactionDTO> typesTransactionsBD = (from EntityModel.MTypeTransaction types
                                                     in contextDB.MTypeTransactions
                                                            select types)
                                                   .ToList()
                                                   .Select(x => x.ToTypeTransactionDTO())
                                                   .ToList();
            return typesTransactionsBD;
        }
    }
}
