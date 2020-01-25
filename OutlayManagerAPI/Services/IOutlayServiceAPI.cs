using OutlayManagerAPI.Model;
using OutlayManagerCore.Model.ResultInfo;
using OutlayManagerCore.Model.Transaction;
using OutlayManagerCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OutlayManagerAPI.Services
{
    public interface IOutlayServiceAPI
    {
        public OutlayServices ConfigureOutlayServices(Action<ConfigurationServices> configuration);

        public List<Transaction> GetAllTransactions();

        public List<Transaction> GetTransactions(int year, int month);

        public Transaction GetTransaction(int id);

        public StateInfo AddTransaction(WSTransaction transaction);

        public StateInfo UpdateTransaction(WSTransaction transaction);

        public StateInfo DeleteTransaction(int id);

        public List<string> GetTypeOutlays();

    }
}
