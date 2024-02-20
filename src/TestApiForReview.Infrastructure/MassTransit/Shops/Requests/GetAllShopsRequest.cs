using TestApiForReview.Infrastructure.Models.Identity;

namespace TestApiForReview.Infrastructure.MassTransit.Shops.Requests
{
    public class GetAllShopsRequest
    {
        public User User { get; set; }
    }
}
