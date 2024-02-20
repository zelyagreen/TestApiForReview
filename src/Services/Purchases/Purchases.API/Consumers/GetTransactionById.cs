﻿using MassTransit;
using Purchases.Domain.Services;
using TestApiForReview.Infrastructure.MassTransit.Purchases.Requests;
using System.Threading.Tasks;

namespace Purchases.API.Consumers
{
    public class GetTransactionById : PurchasesBaseConsumer, IConsumer<GetTransactionByIdRequest>
    {
        public GetTransactionById(IPurchasesService purchasesService) : base(purchasesService)
        {
            
        }

        public async Task Consume(ConsumeContext<GetTransactionByIdRequest> context)
        {
            var order = await PurchasesService.GetTransactionById(context.Message.User, context.Message.Id);
            await context.RespondAsync(order);
        }
    }
}
