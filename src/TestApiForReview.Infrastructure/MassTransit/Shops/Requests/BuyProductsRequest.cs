using System.Collections.Generic;
using TestApiForReview.Infrastructure.Models.Identity;
using TestApiForReview.Infrastructure.Models.Shops;

namespace TestApiForReview.Infrastructure.MassTransit.Shops.Requests
{
    public class BuyProductsRequest
    {
        public User User { get; set; }
        public int ShopId { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
