using TestApiForReview.Infrastructure.Models.Identity;

namespace TestApiForReview.Infrastructure.MassTransit.Purchases.Requests
{
    public class GetTransactionByIdRequest
    {
        public User User { get; set; }
        public int Id { get; set; }
    }
}
