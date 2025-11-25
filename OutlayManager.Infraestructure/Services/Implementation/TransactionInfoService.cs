using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OutlayManager.Model;
using OutlayManagerAPI.Infraestructure.Cache.Abstract;
using OutlayManagerAPI.Infraestructure.Persistence;
using OutlayManagerAPI.Infraestructure.Services.Abstract;
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
        private const string KEY_CACHE_YEARS_AVAILABLES = "YearsAvailables";
        private const string SPENDING_CODE = "SPENDING";
        private const string INCOMING_CODE = "INCOMING";
        private const int PAYROLL_ID = 12;


        private readonly IOutlayDBContext _contextDB;
        private readonly IOutlayManagerCache _cache;
        private readonly ILogger<TransactionInfoService> _log;
        private readonly HashSet<int> _investCodes = new HashSet<int>() { 28, 30, 31 };//RV & Metales & Cryptos

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

        public async Task<List<PayrollPerYearDto>> PayrollPerYearAsync()
        {
            List<TransactionOutlay> paryrollTransactions = await this._contextDB.TransactionOutlay.AsNoTracking()
                                                                                          .Where(x=>x.CodeTransaction_ID == PAYROLL_ID)  
                                                                                          .ToListAsync();

            List<PayrollPerYearDto> payrollPerYear = paryrollTransactions.GroupBy(x => x.DateTransaction.ToDateTime().Year)
                                                                .Select(x =>
                                                                {   return new PayrollPerYearDto()
                                                                    {
                                                                        Year = x.Key,
                                                                        Savings = x.Sum(x=>x.Amount)
                                                                    };
                                                                })
                                                                .OrderBy(x => x.Year)
                                                                .ToList();

                return payrollPerYear;
        }

        public async Task<List<TypeTransactionDTO>> GetTransactionsTypesAsync()
        {
            return await Task.Run(()=> new List<TypeTransactionDTO>() 
            { 
                new TypeTransactionDTO()
                { 
                    ID=1,
                    Type= INCOMING_CODE
                },
                new TypeTransactionDTO()
                {
                    ID=2,
                    Type= SPENDING_CODE
                },
                     
            });
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

        public async Task<List<TransactionResume>> ResumeByTransactionAsync(int year)
        {
            List<TransactionOutlay> transactions = await _contextDB.TransactionOutlay.Include(x => x.TypeTransaction)
                                                                                     .Include(x=>x.CodeTransaction)
                                                                                     .AsNoTracking()
                                                                                     .ToListAsync();

            List<TransactionResume> transactionDTOs = transactions.Select(x => x.ToTransactionDTO())
                                                               .Where(x => x.Date.Year == year)
                                                                .GroupBy(x => x.CodeTransaction)
                                                                .Select(x => new TransactionResume()
                                                                {
                                                                    Code = x.Key,
                                                                    Expenses = x.Where(x => x.TypeTransaction == SPENDING_CODE).Sum(x => x.Amount),
                                                                    Incoming = x.Where(x => x.TypeTransaction == INCOMING_CODE).Sum(x => x.Amount),
                                                                })
                                                                .OrderByDescending(x => x.Expenses + x.Incoming)
                                                               .ToList();

            return transactionDTOs;
        }

        public async Task<MonthResume> MonthResumeAsync(int year, int month)
        {
            List<TransactionOutlay> allTransactions = await _contextDB.TransactionOutlay.AsNoTracking()
                                                                                           .Include(x => x.CodeTransaction)
                                                                                           .Include(x => x.TypeTransaction)
                                                                                           .ToListAsync();
            List<TransactionOutlay> monthTransactions = allTransactions.Where(x=> 
                                                                       {
                                                                           DateTime date = x.DateTransaction.ToDateTime();
                                                                           return date.Year == year && date.Month == month;
                                                                       })
                                                                       .ToList();

            double spendigns = monthTransactions.Where(x => x.TypeTransaction.Type == SPENDING_CODE)
                                                      .Sum(x=>x.Amount);

            double incomings = monthTransactions.Where(x => x.TypeTransaction.Type == INCOMING_CODE)
                                                      .Sum(x => x.Amount);

            return new MonthResume()
            {
                Expenses = spendigns,
                Incomings = incomings,
                Savings = incomings - spendigns,
            };
        }

        public async Task<TotalAmount> TotalAmountAsync()
        {
            List<TransactionOutlay> allTransactions = await _contextDB.TransactionOutlay.AsNoTracking()
                                                                                          .Include(x => x.CodeTransaction)
                                                                                          .Include(x => x.TypeTransaction)
                                                                                          .ToListAsync();

            double spendigns = allTransactions.Where(x => x.TypeTransaction.Type == SPENDING_CODE)
                                                  .Sum(x => x.Amount);

            double incomings = allTransactions.Where(x => x.TypeTransaction.Type == INCOMING_CODE)
                                                      .Sum(x => x.Amount);

            return new TotalAmount()
            {
                Amount = incomings - spendigns
            };
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
