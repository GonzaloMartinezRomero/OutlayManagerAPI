using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OutlayManagerCore.Persistence.Model
{
    public class TransactionOutlay : Entity
    {
        public double Amount { get; set; }
        public string DateTransaction { get; set; }
        public string Description { get; set; }        
        public int CodeTransaction_ID { get; set; }
        [ForeignKey("CodeTransaction_ID")]
        public MCodeTransaction CodeTransaction { get; set; }
        public int TypeTransaction_ID { get; set; }
        [ForeignKey("TypeTransaction_ID")]
        public MTypeTransaction TypeTransaction { get; set; }
    }
}
