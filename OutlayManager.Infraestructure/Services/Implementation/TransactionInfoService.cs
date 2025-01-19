using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OutlayManager.Model.DTO;
using OutlayManagerAPI.Infraestructure.Cache.Abstract;
using OutlayManagerAPI.Infraestructure.Persistence;
using OutlayManagerAPI.Infraestructure.Services.Abstract;
using OutlayManagerAPI.Model.DTO;
using OutlayManagerCore.Infraestructure.Persistence.Model;
using OutlayManagerCore.Infraestructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OutlayManagerAPI.Infraestructure.Services.Implementation
{
    internal class TransactionInfoService : ITransactionInfoService
    {
        private const string KEY_CACHE_TRANSACTION_TYPES = "TransactionTypes";
        private const string KEY_CACHE_YEARS_AVAILABLES = "YearsAvailables";

        private readonly IOutlayDBContext _contextDB;
        private readonly IOutlayManagerCache _cache;
        private readonly ILogger<TransactionInfoService> _log;

        public TransactionInfoService(IOutlayDBContext contextDB, IOutlayManagerCache cache, ILogger<TransactionInfoService> log)
        {
            this._contextDB = contextDB;
            this._cache = cache;
            this._log = log;
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

        public async Task<List<SavingPerYearDto>> SavingsPerYearAsync()
        {
            List<TransactionOutlay> transactions = await this._contextDB.TransactionOutlay.AsNoTracking()
                                                                                          .ToListAsync();

            var transactionTypes = await GetTransactionsTypesAsync();

            int transactionIncomingId = transactionTypes.Where(x => x.Type.ToUpperInvariant() == "INCOMING").Select(x => x.ID).First();
            int transactionExpensesId = transactionTypes.Where(x => x.Type.ToUpperInvariant() == "SPENDING").Select(x => x.ID).First();

            List<SavingPerYearDto> savingsPerYear = transactions.GroupBy(x => x.DateTransaction.ToDateTime().Year)
                                                                .Select(x =>
                                                                {
                                                                    double expenses = x.Where(x => x.TypeTransaction_ID == transactionExpensesId)
                                                                                       .Sum(x => x.Amount);

                                                                    double incomings = x.Where(x => x.TypeTransaction_ID == transactionIncomingId)
                                                                                        .Sum(x => x.Amount);

                                                                    return new SavingPerYearDto()
                                                                    {
                                                                        Year = x.Key,
                                                                        Savings = Math.Round(incomings - expenses, 2)
                                                                    };
                                                                })
                                                                .OrderBy(x => x.Year)
                                                                .ToList();

                return savingsPerYear;
        }

        public async Task<List<TypeTransactionDTO>> GetTransactionsTypesAsync()
        {
            List<TypeTransactionDTO> cachedValues = _cache.GetValue<List<TypeTransactionDTO>>(KEY_CACHE_TRANSACTION_TYPES);

            if (cachedValues == null)
            {
                List<TypeTransactionDTO> typesTransactionsBD = (await (from MTypeTransaction types
                                                            in _contextDB.MTypeTransaction
                                                                         .AsNoTracking()
                                                                       select types)
                                                   .ToListAsync())
                                                   .Select(x => x.ToTypeTransactionDTO())
                                                   .ToList();

                if(!_cache.SetValue<List<TypeTransactionDTO>>(KEY_CACHE_TRANSACTION_TYPES, typesTransactionsBD))
                    _log.LogWarning($"{nameof(GetTransactionsTypesAsync)} could not be cached!");

                return typesTransactionsBD;
            }
            else
            {
                return cachedValues;
            }
        }

        public async Task<List<int>> YearsAvailabes()
        {
            List<int> yearAvailablesCached = _cache.GetValue<List<int>>(KEY_CACHE_YEARS_AVAILABLES);

            if (yearAvailablesCached == null)
            {
                List<int> yearAvailables = (await this._contextDB.TransactionOutlay.AsNoTracking()
                                                                        .Select(x => x.DateTransaction.ToDateTime().Year)
                                                                        .ToListAsync())
                                                                        .Distinct()
                                                                        .ToList();
                
                if(!_cache.SetValue<List<int>>(KEY_CACHE_YEARS_AVAILABLES, yearAvailables))
                      _log.LogWarning($"{nameof(YearsAvailabes)} could not be cached!");

                return yearAvailables;
            }
            else
            {
                return yearAvailablesCached;
            }
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
