using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlayManager.Infraestructure.Model
{
    internal sealed class TransactionMessageServiceBus
    {
        public Guid Id { get; set; } = Guid.Empty;
        public DateTime Date { get; set; }
        public int CodeID { get; set; }
        public int TypeID { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
    }
}
