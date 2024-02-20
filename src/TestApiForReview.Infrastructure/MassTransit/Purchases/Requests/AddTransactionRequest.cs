using TestApiForReview.Infrastructure.Models.Identity;
using TestApiForReview.Infrastructure.Models.Purchases;

namespace TestApiForReview.Infrastructure.MassTransit.Purchases.Requests
{
    public class AddTransactionRequest
    {
        public User User { get; set; }
        public Transaction Transaction { get; set; }
    }
}
