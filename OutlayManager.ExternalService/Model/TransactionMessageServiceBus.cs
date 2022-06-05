using System;

namespace OutlayManager.ExternalService.Model
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
