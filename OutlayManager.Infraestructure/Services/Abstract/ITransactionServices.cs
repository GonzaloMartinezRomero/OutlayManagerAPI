using OutlayManager.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OutlayManagerAPI.Infraestructure.Services.Abstract
{
    public interface ITransactionServices
    {
        /// <summary>
        /// Saves the transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <returns></returns>
        Task<uint> SaveTransaction(TransactionDTO transaction);

        /// <summary>
        /// Deletes the transaction.
        /// </summary>
        /// <param name="transactionID">The transaction identifier.</param>
        /// <returns></returns>
        Task<uint> DeleteTransaction(uint transactionID);

        /// <summary>
        /// Updates the transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <returns></returns>
        Task<TransactionDTO> UpdateTransaction(TransactionDTO transaction);

        /// <summary>
        /// Retrive all transactions
        /// </summary>
        /// <returns></returns>
        Task<List<TransactionDTO>> Transactions();

        /// <summary>
        /// Retrieve all transaction using a year and month as filters
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        Task<List<TransactionDTO>> Transactions(int year, int month);


        /// <summary>
        /// Get transaction by id
        /// </summary>
        /// <param name="transacionID"></param>
        /// <returns></returns>
        Task<TransactionDTO> Transaction(uint transacionID);
        


    }
}
