using TestApiForReview.Infrastructure.Models.Identity;
using TestApiForReview.Infrastructure.Models.Purchases;

namespace TestApiForReview.Infrastructure.MassTransit.Purchases.Requests
{
    public class UpdateTransactionRequest
    {
        public User User { get; set; }
        public UpdateTransaction Transaction { get; set; }
    }
}
