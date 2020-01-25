using System;
using System.Collections.Generic;
using System.Text;
using OutlayManagerCore.Model.Transaction;
using OutlayManagerCore.Persistence.DataBase.EFWithSQLite.EntityModel;

namespace OutlayManagerCore.Persistence.DataBase.EFWithSQLite.Utilities
{
    internal static class Casting
    {
        public static Model.Transaction.Transaction ToModelTransaction(EntityModel.Transaction entityTransaction)
        {
            return (entityTransaction == null) 
                            ? new Model.Transaction.Transaction() 
                            : new Model.Transaction.Transaction()
                            {
                                ID = (uint)entityTransaction.ID,
                                Amount = entityTransaction.Amount,
                                Date = entityTransaction.Date.ToDateTime(),
                                DetailTransaction = ToModelDetailTransaction(entityTransaction.TransactionDetail_FK)
                            };
        }

        public static Model.Transaction.TransactionDetail ToModelDetailTransaction(DetailTransaction transactionDetail_FK)
        {
            if (transactionDetail_FK == null)
                return new TransactionDetail();

            Model.Transaction.TransactionDetail.TypeTransaction typeTransaction;

            if (!Enum.TryParse(transactionDetail_FK.TypeTransaction, out typeTransaction))
                throw new Exception($"Imposible parse {typeTransaction} to TypeTransaction");

            return new TransactionDetail()
            {
                Code = transactionDetail_FK.Code,
                Description = transactionDetail_FK.Description,
                Type = typeTransaction
            };
        }

        public static EntityModel.Transaction ToEntityModel(Model.Transaction.Transaction transaction, string dateFormat)
        {
            return new EntityModel.Transaction()
            {
                Amount = transaction.Amount,
                Date = transaction.Date.ToString(dateFormat),
                TransactionDetail_FK = new DetailTransaction()
                {
                    Code= transaction.DetailTransaction.Code,
                    Description = transaction.DetailTransaction.Description,
                    TypeTransaction = transaction.DetailTransaction.Type.ToString(),
                }
            };
        }
    }
}
