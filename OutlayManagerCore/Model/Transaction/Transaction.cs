using System;
using System.Collections.Generic;
using System.Text;

namespace OutlayManagerCore.Model.Transaction
{
    public class Transaction
    {
        public uint ID { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public TransactionDetail DetailTransaction { get; set; }
    }
}
