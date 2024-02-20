﻿using MassTransit;
using Microsoft.AspNetCore.Mvc;
using TestApiForReview.Infrastructure.Exceptions;
using TestApiForReview.Infrastructure.Filters;
using TestApiForReview.Infrastructure.MassTransit;
using TestApiForReview.Infrastructure.MassTransit.Purchases.Requests;
using TestApiForReview.Infrastructure.Models;
using TestApiForReview.Infrastructure.Models.Identity;
using TestApiForReview.Infrastructure.Models.Purchases;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Purchases.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PurchasesController : ControllerBase
    {
        private readonly IBusControl _busControl;
        private readonly Uri _rabbitMqUrl = new Uri("rabbitmq://localhost/purchasesQueue");

        public PurchasesController(IBusControl busControl)
        {
            _busControl = busControl;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllHistory()
        {
            var user = HttpContext.Items["User"] as User;
            var response = await GetResponseRabbitTask<User, ICollection<Transaction>>(user);
            return Ok(ApiResult<ICollection<Transaction>>.Success200(response));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHistory(int id)
        {
            var user = HttpContext.Items["User"] as User;
            var response = await GetResponseRabbitTask<GetTransactionByIdRequest, Transaction>(new GetTransactionByIdRequest{
            Id = id,
            User = user
            });
            return Ok(ApiResult<Transaction>.Success200(response));
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddTransaction([FromBody] Transaction transaction)
        {
            if (transaction.IsShopCreate)
                throw new BadRequestException("You can't add shops' transaction");
            if (!ModelState.IsValid)
                throw new BadRequestException("Invalid request");
            var user     = HttpContext.Items["User"] as User;
            await GetResponseRabbitTask<AddTransactionRequest, BaseResponseMassTransit>(new AddTransactionRequest()
            {
                User = user,
                Transaction = transaction
            });
            return Ok(ApiResult<int>.Success200(transaction.Id));
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateTransaction( [FromBody] UpdateTransaction updateTransaction)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid request");
            var user = HttpContext.Items["User"] as User;
            await GetResponseRabbitTask<UpdateTransactionRequest, BaseResponseMassTransit>(new UpdateTransactionRequest()
            {
                User = user,
                Transaction = updateTransaction
            });
            return Ok(ApiResult<int>.Success200(updateTransaction.Id));
        }
        private async Task<TOut> GetResponseRabbitTask<TIn, TOut>(TIn request)
            where TIn : class
            where TOut : class
        {
            var client = _busControl.CreateRequestClient<TIn>(_rabbitMqUrl);
            var response = await client.GetResponse<TOut>(request);
            return response.Message;
        }
    }
}
