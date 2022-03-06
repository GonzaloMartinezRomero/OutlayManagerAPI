using Microsoft.EntityFrameworkCore;
using OutlayManagerAPI.Model;
using OutlayManagerAPI.Persistence;
using OutlayManagerCore.Persistence.Model;
using OutlayManagerCore.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OutlayManagerAPI.Services.TransactionServices
{
    internal class TransactionServices : ITransactionServices
    {
        private const string DATE_FORMAT = "dd/MM/yyyy";
        private readonly IOutlayDBContext _contextDB;

        public TransactionServices(IOutlayDBContext contextDB)
        {
            this._contextDB = contextDB;
        }

        public async Task<uint> SaveTransaction(TransactionDTO transaction)
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

            await _contextDB.TransactionOutlay.AddAsync(newTransaction);
            await _contextDB.Context.SaveChangesAsync();

            //load by ref the ID given by the DB
            transaction.ID = (uint)newTransaction.ID;

            return transaction.ID;
        }

        public async Task<TransactionDTO> UpdateTransaction(TransactionDTO transaction)
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

            await _contextDB.Context.SaveChangesAsync();

            return transactionToUpdate.ToTransactionDTO();
        }

        public async Task<TransactionDTO> Transaction(uint transactionID)
        {
            TransactionDTO transaction = (await (from TransactionOutlay t
                                                 in _contextDB.TransactionOutlay.AsNoTracking()
                                                             .AsNoTracking()
                                                             .Include(x => x.TypeTransaction)
                                                             .Include(x => x.CodeTransaction)
                                                 where t.ID == transactionID
                                                 select t)
                                                 .SingleOrDefaultAsync())
                                                 .ToTransactionDTO();

            return transaction;
        }

        public async Task<List<TransactionDTO>> Transactions(int year , int month)
        {
            bool loadCompleteYear = month == 0;

            List<TransactionDTO> transactionsBD = (await(from TransactionOutlay t
                                                   in _contextDB.TransactionOutlay
                                                               .AsNoTracking()
                                                               .Include(x => x.TypeTransaction)
                                                               .Include(x => x.CodeTransaction)
                                                   select t)
                                                   .ToListAsync())
                                                   .Where(t =>
                                                   {
                                                       DateTime dateParsed = t.DateTransaction.ToDateTime();

                                                       if (loadCompleteYear)
                                                           return dateParsed.Year == year;
                                                       else
                                                           return dateParsed.Year == year && dateParsed.Month == month;
                                                   })
                                                   .Select(x => x.ToTransactionDTO())
                                                   .ToList();

            return transactionsBD;
        }

        public async Task<List<TransactionDTO>> Transactions()
        {
            List<TransactionDTO> transactionsBD = (await(from TransactionOutlay t
                                                    in _contextDB.TransactionOutlay.AsNoTracking()
                                                                .AsNoTracking()
                                                                .Include(x => x.TypeTransaction)
                                                                .Include(x => x.CodeTransaction)
                                                   select t)
                                                   .ToListAsync())
                                                   .Select(x => x.ToTransactionDTO())
                                                   .ToList();

            return transactionsBD;
        }

        public async Task<uint> DeleteTransaction(uint transactionID)
        {
            TransactionOutlay transactionToDelete = await (from TransactionOutlay t
                                                    in _contextDB.TransactionOutlay
                                                     where t.ID == transactionID
                                                     select t)
                                                          .SingleOrDefaultAsync() ?? throw new Exception($"[{nameof(DeleteTransaction)}]: Transaction ID:{transactionID} not found");


            _contextDB.TransactionOutlay.Remove(transactionToDelete);
            await _contextDB.Context.SaveChangesAsync();

            return (uint)transactionToDelete.ID;
        }
    }
}
