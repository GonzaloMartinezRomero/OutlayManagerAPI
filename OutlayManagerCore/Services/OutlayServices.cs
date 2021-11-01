using OutlayManagerCore.Persistence;
using OutlayManagerCore.Persistence.DataBase.EFWithSQLite;
using System;

namespace OutlayManagerCore.Services
{
    public class OutlayServices : IDisposable
    {
        public ITransactionsServices TransactionService { get { return this._transactionsServices; } }

        private readonly ITransactionsServices _transactionsServices;

        private OutlayServices() { }

        public OutlayServices (Action<ConfigurationServices> configure)
        {   
           ConfigurationServices configurationServices = new ConfigurationServices();
            configure.Invoke(configurationServices);

            switch (configurationServices.Provider)
            {
                case ConfigurationServices.TypesProviders.SQLITE_ON_EF:
                    _transactionsServices = new SqliteEFManager(configurationServices.PathConnection);
                    break;

                default:
                    throw new Exception($"{configurationServices.Provider} not suported!");
            }
        }

        public void Dispose()
        {
            TransactionService.Dispose();
        }
    }
}
