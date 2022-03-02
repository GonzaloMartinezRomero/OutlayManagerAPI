using Microsoft.EntityFrameworkCore;
using OutlayManagerAPI.Model;
using OutlayManagerAPI.Persistence;
using OutlayManagerCore.Persistence.Model;
using OutlayManagerCore.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OutlayManagerAPI.Services.TransactionInfoServices
{
    public class TransactionInfoService : ITransactionInfoService
    {
        private readonly IDBEntityContext _contextDB;

        public TransactionInfoService(IDBEntityContext contextDB)
        {
            this._contextDB = contextDB;
        }

        public async Task<List<AmountResume>> AmountResumes()
        {
            List<AmountResume> amountResumes = new List<AmountResume>();

            List<TransactionOutlay> transactions = await _contextDB.TransactionOutlay.Include(x=>x.TypeTransaction)
                                                                                     .AsNoTracking()
                                                                                     .ToListAsync();

            var transactionsGroup = transactions.GroupBy(key =>
            {
                TransactionDTO transactionAux = key.ToTransactionDTO();
                TransactionKey trKey = new TransactionKey(transactionAux.Date.Year,
                                                          transactionAux.Date.Month);

                return trKey;
            });

            double currentTotalAmount = 0.0d;

            foreach (var transactionGroupAux in transactionsGroup)
            {
                foreach(var transactionsAux in transactionGroupAux)
                {
                    switch (transactionsAux.TypeTransaction.Type)
                    {
                        case "SPENDING":
                            currentTotalAmount -= transactionsAux.Amount;
                            break;

                        default:
                            currentTotalAmount += transactionsAux.Amount;
                            break;
                    }
                }

                TransactionKey key = transactionGroupAux.Key;

                amountResumes.Add(new AmountResume()
                {
                    Year = key.Year,
                    Month = key.Month,
                    Amount = currentTotalAmount
                });
            }

            return amountResumes;
        }

        public async Task<List<TypeTransactionDTO>> TransactionsTypes()
        {
            List<TypeTransactionDTO> typesTransactionsBD = (await (from MTypeTransaction types
                                                            in _contextDB.MTypeTransaction
                                                                         .AsNoTracking()
                                                            select types)
                                                   .ToListAsync())
                                                   .Select(x => x.ToTypeTransactionDTO())
                                                   .ToList();

            return typesTransactionsBD;
        }

        public async Task<List<int>> YearsAvailabes()
        {
            List<int> yearAvailables =  (await this._contextDB.TransactionOutlay.AsNoTracking()
                                                                        .Select(x => x.DateTransaction.ToDateTime().Year)
                                                                        .ToListAsync())
                                                                        .Distinct()
                                                                        .ToList();

            return yearAvailables;
        }

        private sealed class TransactionKey
        {
            public int Year { get; set; }
            public int Month { get; set; }

            private TransactionKey() { }

            public TransactionKey(int year, int month) 
            {
                this.Year = year;
                this.Month = month;
            }

            public override bool Equals(object obj)
            {
                if(obj is TransactionKey transactionAux)
                {
                    return this.Year == transactionAux.Year &&
                           this.Month == transactionAux.Month;
                }

                return false;
            }

            public override int GetHashCode()
            {
                return this.ToString().GetHashCode();
            }

            public override string ToString()
            {
                return $"{Year.ToString()}{Month.ToString()}";
            }

        }
    }
}
