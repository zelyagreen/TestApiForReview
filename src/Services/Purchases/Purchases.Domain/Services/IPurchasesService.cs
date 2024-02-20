using System.Collections.Generic;
using System.Threading.Tasks;
using TestApiForReview.Infrastructure.MassTransit;
using TestApiForReview.Infrastructure.Models.Identity;
using TestApiForReview.Infrastructure.Models.Purchases;

namespace Purchases.Domain.Services
{
    public interface IPurchasesService
    {
        public Task<Transaction> GetTransactionById(User user, int id);
        public Task<ICollection<Transaction>> GetTransactions(User user);
        public Task<BaseResponseMassTransit> AddTransaction(User user, Transaction transaction);
        public Task<BaseResponseMassTransit> UpdateTransaction(User user, UpdateTransaction transaction);
    }
}
