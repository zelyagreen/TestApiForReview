using System.Collections.Generic;
using TestApiForReview.Infrastructure.Models.Shops;

namespace TestApiForReview.Infrastructure.MassTransit.Shops.Requests
{
    public class AddProductsByFactoryRequest
    {
        public List<ProductByFactory> Products { get; set; }
    }
}
