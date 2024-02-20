using System.Collections.Generic;
using TestApiForReview.Infrastructure.Models.Purchases;

namespace TestApiForReview.Infrastructure.MassTransit.Purchases.Responses
{
    public class GetTransactionsResponse
    {
        public List<Transaction> Transactions { get; set; }
        public int Count { get; set; }
    }
}
