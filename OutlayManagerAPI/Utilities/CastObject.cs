using OutlayManagerAPI.Model;
using OutlayManagerCore.Model.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OutlayManagerAPI.Utilities
{
    public static  class CastObject
    {
        public static WSTransaction ToWSTransaction(Transaction outlayCoreTransaction)
        {
            return (outlayCoreTransaction == null)
                ? new WSTransaction() { DetailTransaction = new WSDetailTransaction() }
                : new WSTransaction()
                {
                    ID = (int)outlayCoreTransaction.ID,
                    Amount = outlayCoreTransaction.Amount,
                    Date = outlayCoreTransaction.Date,
                    DetailTransaction = new WSDetailTransaction()
                    {
                        Code = outlayCoreTransaction.DetailTransaction.Code,
                        Description = outlayCoreTransaction.DetailTransaction.Description,
                        Type = outlayCoreTransaction.DetailTransaction.Type.ToString()
                    }
                };
        }

        public static Transaction ToTransaction(WSTransaction wsTransaction)
        {
            return new Transaction()
            {
                ID = (uint)wsTransaction.ID,
                Amount = wsTransaction.Amount,
                Date = wsTransaction.Date,
                DetailTransaction = new TransactionDetail()
                {
                    Code = wsTransaction.DetailTransaction.Code,
                    Description = wsTransaction.DetailTransaction.Description,
                    Type = (TransactionDetail.TypeTransaction)Enum.Parse(typeof(TransactionDetail.TypeTransaction), wsTransaction.DetailTransaction.Type)
                }
            };
        }
    }
}
