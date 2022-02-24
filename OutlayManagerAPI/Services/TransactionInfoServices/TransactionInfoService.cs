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
    }
}
