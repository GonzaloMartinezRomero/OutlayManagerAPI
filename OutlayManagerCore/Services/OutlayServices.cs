using OutlayManagerCore.Manager.ExportDataManager;
using OutlayManagerCore.Manager.OutlayManager;
using OutlayManagerCore.Model.ResultInfo;
using OutlayManagerCore.Model.Transaction;
using OutlayManagerCore.Persistence;
using OutlayManagerCore.Persistence.DataBase;
using OutlayManagerCore.Persistence.DataBase.EFWithSQLite;
using OutlayManagerCore.Persistence.MemoryData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity;
using Unity.Lifetime;
using Unity.Resolution;

namespace OutlayManagerCore.Services
{
    public class OutlayServices : IDisposable
    {
        public enum FormatSecurityCopy
        {
            CSV,
            EXCEL
        }
             
        private OutlayManager managerServices = null;

        private IUnityContainer unityIOC = null;
        private ConfigurationServices configurationServices = null;     

        public OutlayServices OnConfigure(Action<ConfigurationServices> configure)
        {
            if (configurationServices == null)
            {
                configurationServices = new ConfigurationServices();
                configure.Invoke(configurationServices);
            }   
            else
                throw new Exception("Configuration has been already setted");
            
            return this;
        }

        public OutlayServices InitialiceComponents()
        {
            if (unityIOC == null)
            {
                unityIOC = new UnityContainer();

                switch (configurationServices.Provider)
                {
                    case ConfigurationServices.TypesProviders.MEMORY:
                        unityIOC.RegisterType<IPersistenceServices, MemoryDataPersistence>(new ContainerControlledLifetimeManager());//Singletone
                        managerServices = unityIOC.Resolve<OutlayManager>();
                        break;

                    case ConfigurationServices.TypesProviders.SQLITE_ON_EF:
                        unityIOC.RegisterType<IPersistenceServices, SqliteEFManager>(new PerResolveLifetimeManager()); //Create new object and reuse 
                        managerServices = unityIOC.Resolve<OutlayManager>(new ResolverOverride[]
                        {
                            new ParameterOverride("pathDataSource",configurationServices.PathConnection)
                        });
                        break;

                    default:
                        throw new Exception("Type provider unknown");
                }
            }
            else
            {
                throw new Exception("Is already initiliced!");
            }

            return this;
        }

        public StateInfo InsertNewTransaction(Transaction transaction)
        {
            return managerServices?.AddTransaction(transaction) ?? throw new Exception("Service not initialice");
        }

        public StateInfo UpdateTransaction(Transaction transaction)
        {
            return managerServices?.UpdateTransaction(transaction);
        }

        public IEnumerable<Transaction> GetAllTransactions()
        {
            return managerServices?.GetAllTransactions() ?? throw new Exception("Service not initialice");
        }

        public IEnumerable<Transaction> GetTransactions(int year, int month)
        {
            return managerServices?.GetTransactions(year, month) ?? throw new Exception("Service not initialice"); 
        }

        public Transaction GetTransaction(uint transactionID)
        {
            return managerServices?.GetTransaction(transactionID) ?? throw new Exception("Service not initialice"); ;
        }

        public StateInfo DeleteTransaction(uint transactionID)
        {
            return managerServices?.DeleteTransaction(transactionID) ?? throw new Exception("Service not initialice"); ;
        }

        public List<string> GetTypeOutlays()
        {
            return Enum.GetNames(typeof(TransactionDetail.TypeTransaction)).ToList();
        }

        public void CreateBackup(FormatSecurityCopy format, string path)
        {
            switch (format)
            {
                case FormatSecurityCopy.CSV:
                    ExportDataManager.ExportToCSV(path);
                    break;

                case FormatSecurityCopy.EXCEL:
                    ExportDataManager.ExportToExcel(path);
                    break;

                default:
                    throw new Exception("Format not recogniced");
            }
        }

        public void Dispose()
        {
            if (unityIOC != null)
                unityIOC.Dispose();
        }
    }
}
