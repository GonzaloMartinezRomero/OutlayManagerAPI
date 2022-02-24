using Microsoft.EntityFrameworkCore;
using OutlayManagerAPI.Model;
using OutlayManagerAPI.Persistence;
using OutlayManagerCore.Persistence.Model;
using OutlayManagerCore.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OutlayManagerAPI.Services.TransactionCodeService
{
    public class TransactionCodeService : ITransactionCodeService
    {
        private readonly IDBEntityContext _contextDB;

        public TransactionCodeService(IDBEntityContext contextDB)
        {
            this._contextDB = contextDB;
        }

        public async Task<int> AddTransactionCode(CodeTransactionDTO codeTransactionDTO)
        {
            string transactionCodeFormat = codeTransactionDTO.Code.ToUpperInvariant().Trim();

            HashSet<string> codesAvailables = this._contextDB.MCodeTransaction.Select(x => x.Code).ToHashSet();

            if (codesAvailables.Contains(transactionCodeFormat))
                throw new Exception($"The code {transactionCodeFormat} already exist!");

            MCodeTransaction mCodeTransaction = new MCodeTransaction()
            {
                Code = transactionCodeFormat
            };

            await this._contextDB.MCodeTransaction.AddAsync(mCodeTransaction);
            await this._contextDB.Context.SaveChangesAsync();

            return mCodeTransaction.ID;
        }

        public async Task<int> DeleteTransactionCode(uint id)
        {
            MCodeTransaction mCodeTransaction = this._contextDB.MCodeTransaction.Where(x => x.ID == id).SingleOrDefault();

            if (mCodeTransaction == null)
                throw new Exception($"Code ID: {id} not found!");

            int transactionCodesInUse = this._contextDB.TransactionOutlay.Where(x => x.CodeTransaction_ID == mCodeTransaction.ID).Count();

            if (transactionCodesInUse > 0)
                throw new Exception("Not possible delete due to is being using in a transactions");

            this._contextDB.Context.Remove(mCodeTransaction);
            await this._contextDB.Context.SaveChangesAsync();

            return mCodeTransaction.ID;
        }

        public async Task<List<CodeTransactionDTO>> TransactionsCodes()
        {
            List<CodeTransactionDTO> codesTransactionsBD = (await (from MCodeTransaction codes
                                                     in _contextDB.MCodeTransaction.AsNoTracking()
                                                            select codes)
                                                   .ToListAsync())
                                                   .Select(x => x.ToCodeTransactionDTO())
                                                   .ToList();
            return codesTransactionsBD;
        }
    }
}
