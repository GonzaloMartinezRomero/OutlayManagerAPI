using Microsoft.EntityFrameworkCore;
using OutlayManagerAPI.Model;
using OutlayManagerCore.Persistence.DataBase.EFWithSQLite;
using OutlayManagerCore.Persistence.DataBase.EFWithSQLite.EntityModel;
using OutlayManagerCore.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OutlayManagerAPI.Services.TransactionServices
{
    internal class SQLiteTransactionServices : ITransactionServices
    {
        private const string DATE_FORMAT = "dd/MM/yyyy";
        private readonly SQLiteContext _contextDB;

        public SQLiteTransactionServices(SQLiteContext contextDB)
        {
            this._contextDB = contextDB;
        }
      
        public void SaveTransaction(TransactionDTO transaction)
        {
            if (transaction.CodeTransactionID <= 0)
                throw new Exception($"[{nameof(SaveTransaction)}] {nameof(TransactionDTO.CodeTransactionID)} is mandatory");

            if (transaction.TypeTransactionID <= 0)
                throw new Exception($"[{nameof(SaveTransaction)}] {nameof(TransactionDTO.TypeTransactionID)} is mandatory");

            var newTransaction = new TransactionOutlay()
            {
                Amount = transaction.Amount,
                DateTransaction = transaction.Date.ToString(DATE_FORMAT),
                Description = transaction.Description,
                CodeTransaction_ID = transaction.CodeTransactionID,
                TypeTransaction_ID = transaction.TypeTransactionID
            };

            _contextDB.TransactionOutlay.Add(newTransaction);
            _contextDB.SaveChanges();

            transaction.ID = (uint)newTransaction.ID;
        }

        public void UpdateTransaction(TransactionDTO transaction)
        {
            TransactionOutlay transactionToUpdate = (from TransactionOutlay t
                                                                  in _contextDB.TransactionOutlay
                                                                              .Include(x => x.TypeTransaction)
                                                                              .Include(x => x.CodeTransaction)
                                                                  where t.ID == transaction.ID
                                                                  select t).SingleOrDefault();

            if (transactionToUpdate == null)
                throw new Exception($"[{nameof(UpdateTransaction)}] Requested transaction not exist in DB. ID:{transaction.ID}");
           
            transactionToUpdate.Amount = transaction.Amount;
            transactionToUpdate.DateTransaction = transaction.Date.ToString(DATE_FORMAT);
            transactionToUpdate.CodeTransaction_ID = transaction.CodeTransactionID;
            transactionToUpdate.Description = transaction.Description;
            transactionToUpdate.TypeTransaction_ID = transaction.TypeTransactionID;

            _contextDB.SaveChanges();            
        }

        public TransactionDTO GetTransaction(uint transactionID)
        {
            TransactionDTO transaction = (from TransactionOutlay t
                                                            in _contextDB.TransactionOutlay.AsNoTracking()
                                                                        .AsNoTracking()
                                                                        .Include(x => x.TypeTransaction)
                                                                        .Include(x => x.CodeTransaction)
                                                            where t.ID == transactionID
                                                            select t)
                                                            .SingleOrDefault()
                                                            .ToTransactionDTO();

            return transaction;
        }

        public List<TransactionDTO> GetTransactions(int year, int month)
        {
            List<TransactionDTO> transactionsBD = (from TransactionOutlay t
                                                   in _contextDB.TransactionOutlay.AsNoTracking()
                                                               .AsNoTracking()
                                                               .Include(x => x.TypeTransaction)
                                                               .Include(x => x.CodeTransaction)
                                                   select t)
                                                   .ToList()
                                                   .Where(t =>
                                                   {
                                                       DateTime dateParsed = t.DateTransaction.ToDateTime();
                                                       return dateParsed.Year == year && dateParsed.Month == month;
                                                   })
                                                   .Select(x => x.ToTransactionDTO())
                                                   .ToList();

            return transactionsBD;
        }

        public List<TransactionDTO> GetAllTransactions()
        {
            List<TransactionDTO> transactionsBD = (from TransactionOutlay t
                                                    in _contextDB.TransactionOutlay.AsNoTracking()
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
            TransactionOutlay transactionToDelete = (from TransactionOutlay t
                                                          in _contextDB.TransactionOutlay
                                                     where t.ID == transactionID
                                                     select t)
                                                          .SingleOrDefault() ?? throw new Exception($"[{nameof(DeleteTransaction)}]: Transaction ID:{transactionID} not found");


            _contextDB.TransactionOutlay.Remove(transactionToDelete);
            _contextDB.SaveChanges();        
        }
      
        public List<CodeTransactionDTO> GetMCodeTransactions()
        {
            List<CodeTransactionDTO> codesTransactionsBD = (from MCodeTransaction codes
                                                     in _contextDB.MCodeTransaction.AsNoTracking()
                                                select codes)
                                                   .ToList()
                                                   .Select(x=>x. ToCodeTransactionDTO())
                                                   .ToList();
            return codesTransactionsBD;
        }

        public List<TypeTransactionDTO> GetMTypeTransactions()
        {
            List<TypeTransactionDTO> typesTransactionsBD = (from MTypeTransaction types
                                                            in _contextDB.MTypeTransaction
                                                                         .AsNoTracking()
                                                select types)
                                                   .ToList()
                                                   .Select(x=>x.ToTypeTransactionDTO())
                                                   .ToList();
                                                   
            return typesTransactionsBD;
        }

        public void AddTransactionCode(CodeTransactionDTO codeTransactionDTO)
        {
            string transactionCodeFormat = codeTransactionDTO.Code.ToUpperInvariant().Trim();
            
            HashSet<string> codesAvailables = this._contextDB.MCodeTransaction.Select(x => x.Code).ToHashSet();

            if (codesAvailables.Contains(transactionCodeFormat))
                throw new Exception($"The code {transactionCodeFormat} already exist!");

            MCodeTransaction mCodeTransaction = new MCodeTransaction()
            {
                Code = transactionCodeFormat
            };

            this._contextDB.MCodeTransaction.Add(mCodeTransaction);
            this._contextDB.SaveChanges();
        }

        public void DeleteTransactionCode(uint id)
        {
            MCodeTransaction mCodeTransaction = this._contextDB.MCodeTransaction.Where(x => x.ID == id).SingleOrDefault();

            if (mCodeTransaction == null)
                throw new Exception($"Code ID: {id} not found!");

            int transactionCodesInUse = this._contextDB.TransactionOutlay.Where(x => x.CodeTransaction_ID == mCodeTransaction.ID).Count();

            if (transactionCodesInUse > 0)
                throw new Exception("Not possible delete due to is being using in a transactions");

            this._contextDB.Remove(mCodeTransaction);
            this._contextDB.SaveChanges();
        }

        public List<int> GetYearsAvailabes()
        {
            List<int> yearAvailables = this._contextDB.TransactionOutlay.AsNoTracking()
                                                                        .Select(x => x.DateTransaction.ToDateTime().Year)
                                                                        .ToList()
                                                                        .Distinct()
                                                                        .ToList();

            return yearAvailables;
        }
    }
}
