using OutlayManagerAPI.Model;
using OutlayManagerCore.Persistence.Model;
using System;

namespace OutlayManagerCore.Utilities
{
    public static class ConverterExtensionMethod
    {
        public static DateTime ToDateTime(this string str)
        {
            DateTime result = default;

            if (!String.IsNullOrEmpty(str) && !DateTime.TryParse(str, out result))
                throw new Exception("Imposible to cast to DateTime");

            return result;
        }

        public static TransactionDTO ToTransactionDTO(this TransactionOutlay entityTransaction)
        {
            return (entityTransaction == null)
                            ? new TransactionDTO()
                            : new TransactionDTO()
                            {
                                ID = (uint)entityTransaction.ID,
                                Amount = entityTransaction.Amount,
                                Date = entityTransaction.DateTransaction.ToDateTime(),
                                CodeTransaction = entityTransaction.CodeTransaction?.Code,
                                CodeTransactionID = entityTransaction.CodeTransaction_ID,
                                TypeTransaction = entityTransaction.TypeTransaction?.Type,
                                TypeTransactionID = entityTransaction.TypeTransaction_ID,
                                Description = entityTransaction.Description
                            };
        }

        public static CodeTransactionDTO ToCodeTransactionDTO(this MCodeTransaction entityCodeTransaction)
        {
            return (entityCodeTransaction == null)
                        ? new CodeTransactionDTO()
                        : new CodeTransactionDTO()
                        {
                            ID = entityCodeTransaction.ID,
                            Code = entityCodeTransaction.Code
                        };

        }

        public static TypeTransactionDTO ToTypeTransactionDTO(this MTypeTransaction entityTypeTransaction)
        {
            return (entityTypeTransaction == null)
                        ? new TypeTransactionDTO()
                        : new TypeTransactionDTO()
                        {
                            ID = entityTypeTransaction.ID,
                            Type = entityTypeTransaction.Type
                        };

        }

        public static UInt32 ToUInt32(this Int32 int32Value)
        {
            if (int32Value < 0)
                return 0;

            return (UInt32)int32Value;
        }
    }
}
