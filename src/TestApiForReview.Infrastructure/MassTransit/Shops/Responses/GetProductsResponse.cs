using System.Collections.Generic;
using TestApiForReview.Infrastructure.Models.Shops;

namespace TestApiForReview.Infrastructure.MassTransit.Shops.Responses
{
    public class GetProductsResponse
    {
        public bool Success { get; set; }
        public List<Product> Products { get; set; }
    }
}
