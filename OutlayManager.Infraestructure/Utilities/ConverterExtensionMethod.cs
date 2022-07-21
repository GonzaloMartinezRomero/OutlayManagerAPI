using OutlayManagerAPI.Model.DTO;
using OutlayManagerCore.Infraestructure.Persistence.Model;
using System;
using System.Globalization;

namespace OutlayManagerCore.Infraestructure.Utilities
{
    internal static class ConverterExtensionMethod
    {
        private const string DATE_FORMAT = "dd/MM/yyyy";
        private static IFormatProvider DATE_FORMAT_PROVIDER = CultureInfo.InvariantCulture;        

        public static DateTime ToDateTime(this string str)
        {
            DateTime dateTimeParsed = default;

            if (!DateTime.TryParseExact(str, DATE_FORMAT, DATE_FORMAT_PROVIDER, DateTimeStyles.None, out dateTimeParsed))
                throw new ArgumentException("Imposible to cast to DateTime", str);

            return dateTimeParsed;
        }

        public static TransactionDTO ToTransactionDTO(this TransactionOutlay entityTransaction)
        {
            if(entityTransaction!=null)
                return new TransactionDTO()
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

            return null;
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
