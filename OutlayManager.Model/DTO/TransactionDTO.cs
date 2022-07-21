using System;

namespace OutlayManagerAPI.Model.DTO
{
    public class TransactionDTO
    {
        public uint ID { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int CodeTransactionID { get; set; }
        public string CodeTransaction { get; set; }
        public int TypeTransactionID { get; set; }
        public string TypeTransaction { get; set; }

        public override string ToString()
        {
            return $"{ID}|{Amount}|{Date.ToString("dd/MM/yyyy")}|{CodeTransactionID}:{CodeTransaction}|{TypeTransactionID}:{TypeTransaction}|{Description}";
        }
    }
}
