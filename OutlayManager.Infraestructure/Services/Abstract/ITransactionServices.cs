using OutlayManagerAPI.Model.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OutlayManagerAPI.Infraestructure.Services.Abstract
{
    public interface ITransactionServices
    {
        Task<uint> SaveTransaction(TransactionDTO transaction);
        Task<uint> DeleteTransaction(uint transactionID);
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
