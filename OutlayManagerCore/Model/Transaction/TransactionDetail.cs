using System;
using System.Collections.Generic;
using System.Text;

namespace OutlayManagerCore.Model.Transaction
{
    public class TransactionDetail
    {
        public enum TypeTransaction
        {
            SPENDING,
            ADJUST,
            INCOMING
        }

        public string Code { get; set; }
        public string Description { get; set; }
        public TypeTransaction Type { get; set; }
    }
}
