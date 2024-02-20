using System.Threading.Tasks;
using MassTransit;
using TestApiForReview.Infrastructure.MassTransit.Shops.Requests;
using Shops.Domain.Services;

namespace Shops.API.Consumers
{
    public class GetAllShops : ShopsBaseConsumer, IConsumer<GetAllShopsRequest>
    {
        public GetAllShops(IShopsService shopsService) : base(shopsService)
        {
        }
        public async Task Consume(ConsumeContext<GetAllShopsRequest> context)
        {
            var order = ShopsService.GetAllShops();
            await context.RespondAsync(order);
        }
    }
}
