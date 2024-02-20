using System.Threading.Tasks;
using MassTransit;
using TestApiForReview.Infrastructure.MassTransit.Shops.Requests;
using Shops.Domain.Services;

namespace Shops.API.Consumers
{
    public class GetProductsByShop : ShopsBaseConsumer, IConsumer<GetProductsRequest>
    {
        public GetProductsByShop(IShopsService shopsService) : base(shopsService)
        {
        }

        public async Task Consume(ConsumeContext<GetProductsRequest> context)
        {
            var products = await ShopsService.GetProductsByShop(context.Message.ShopId);
            await context.RespondAsync(products);
        }
    }
}
