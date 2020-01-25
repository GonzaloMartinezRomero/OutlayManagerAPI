using OutlayManagerAPI.Model;
using OutlayManagerAPI.Utilities;
using OutlayManagerCore.Model.ResultInfo;
using OutlayManagerCore.Model.Transaction;
using OutlayManagerCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OutlayManagerAPI.Services
{
    public class OutlayServiceAPI : IOutlayServiceAPI
    {
        private readonly OutlayServices outlayCore;

        public OutlayServiceAPI()
        {
            outlayCore = new OutlayServices();
        }

        public OutlayServices ConfigureOutlayServices(Action<ConfigurationServices> configuration) => outlayCore.OnConfigure(configuration);

        public List<Transaction> GetAllTransactions()
        {
            return outlayCore.GetAllTransactions().ToList();
        }

        public List<Transaction> GetTransactions(int year, int month)
        {
            return outlayCore.GetTransactions(year, month).ToList();
        }

        public Transaction GetTransaction(int id)
        {
            return outlayCore.GetTransaction((uint)id);
        }

        public StateInfo UpdateTransaction(WSTransaction transaction)
        {
            Transaction transactionCore = CastObject.ToTransaction(transaction);
            return outlayCore.UpdateTransaction(transactionCore);
        }

        public StateInfo AddTransaction(WSTransaction transaction)
        {
            Transaction transactionCore = CastObject.ToTransaction(transaction);
            return outlayCore.InsertNewTransaction(transactionCore);
        }

        public StateInfo DeleteTransaction(int id)
        {
            return outlayCore.DeleteTransaction((uint)id);
        }

        public List<string> GetTypeOutlays()
        {
            return outlayCore.GetTypeOutlays();
        }
    }
}
