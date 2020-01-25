using System;
using System.Collections.Generic;
using System.Text;

namespace OutlayManagerCore.Persistence.DataBase.EFWithSQLite.EntityModel
{
    public class Transaction : Entity
    {   
        public double Amount { get; set; }
        public string Date { get; set; }
        public int TransactionDetail_FKID { get; set; }
        public DetailTransaction TransactionDetail_FK { get; set; }
    }
}
