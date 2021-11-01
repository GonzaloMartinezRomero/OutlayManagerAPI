using System;
using System.Collections.Generic;
using System.Text;

namespace OutlayManagerAPI.Model
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
    }
}
