using System;
using OutlayManagerCore.Services;
using OutlayManagerCore.Model.ResultInfo;
using OutlayManagerCore.Model.Transaction;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace OutlayManagerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            using (OutlayServices outlayServices = new OutlayServices())
            {
               outlayServices.OnConfigure(x =>
                             {
                                 x.Provider = ConfigurationServices.TypesProviders.SQLITE_ON_EF;
                                 x.PathConnection = @"D:\Instalacion_SQLite\Data\OutlayManagerDataBase.db";
                             })
                             .InitialiceComponents();

                outlayServices.DeleteTransaction(1);


                StateInfo state = outlayServices.InsertNewTransaction(new TransactionDTO()
                {
                    Amount = 15.20d,
                    Date = new DateTime(2019, 09, 25),
                    DetailTransaction = new OutlayManagerCore.Model.Transaction.TransactionDetail()
                    {
                        Code = "Mercadona",
                        Description = "Puta compra",
                        Type = OutlayManagerCore.Model.Transaction.TransactionDetail.TypeTransaction.SPENDING
                    }
                });

                if (state.ResultState == StateInfo.State.SUCCESS)
                    Console.WriteLine("All ok!");
                else
                    Console.WriteLine("All bad");


                var myTransactions = outlayServices.GetTransactions(2019, 09).ToList();

                StringBuilder strBuilder = new StringBuilder();

                foreach (var transactionAux in myTransactions)
                {
                    Console.WriteLine("----------------------------");
                    Console.WriteLine($"{nameof(transactionAux.ID)}: {transactionAux.ID}");
                    Console.WriteLine($"{nameof(transactionAux.DetailTransaction.Type)}: {transactionAux.DetailTransaction.Type}");
                    Console.WriteLine($"{nameof(transactionAux.Amount)}: {transactionAux.Amount}");
                    Console.WriteLine($"{nameof(transactionAux.Date)}: {transactionAux.Date.ToShortDateString()}");
                    Console.WriteLine($"{nameof(transactionAux.DetailTransaction.Code)}: {transactionAux.DetailTransaction.Code}");
                    Console.WriteLine($"{nameof(transactionAux.DetailTransaction.Description)}: {transactionAux.DetailTransaction.Description}");

                    Console.WriteLine("----------------------------");
                }

                TransactionDTO transactionDelete = myTransactions.Where(x => x.ID == state.ReturnedID).Single();
                var statusReturned = outlayServices.DeleteTransaction(transactionDelete.ID);

                if (statusReturned.ResultState == StateInfo.State.SUCCESS)
                    Console.WriteLine($"Deleted transacion {statusReturned.ReturnedID} !");
                else
                    Console.WriteLine("All bad");
              
            }

            Console.ReadKey();

        }
    }
}
