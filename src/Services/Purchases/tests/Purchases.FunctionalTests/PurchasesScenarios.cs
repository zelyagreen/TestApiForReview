﻿using Identity.FunctionalTests.Base;
using Microsoft.AspNetCore.TestHost;
using Purchases.FunctionalTests.Base;
using TestApiForReview.Infrastructure.Models;
using TestApiForReview.Infrastructure.Models.Identity;
using TestApiForReview.Infrastructure.Models.Purchases;
using TestApiForReview.Infrastructure.Models.Shops;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Purchases.FunctionalTests
{
    public class PurchasesScenarios : PurchasesScenariosBase
    {
        private const string RequestType = "application/json";
        private string _token;
        private TestServer _testServerIdentity;
        private TestServer _thisServer;
        public PurchasesScenarios()
        {
            Configure().Wait();
        }
        private async Task Configure()
        {
            _testServerIdentity = new Identity.FunctionalTests.IdentityScenarios().CreateServer();
            _token = await GetToken();
            _thisServer = CreateServer();
        }

        [Fact]
        public async Task Get_All_History_User_response_ok_status_code()
        {
            using var client = _thisServer.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization",$"Bearer {_token}");
            var response = await client.GetAsync(Get.AllHistory);
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Get_Transaction_User_By_Id_response_ok_status_code()
        {
            using var client = _thisServer.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
            var response = await client.GetAsync(Get.TransactionById);
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Put_Update_Transaction_response_ok_status_code()
        {
            using var client = _thisServer.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
            var content = new StringContent(BuildUpdateTransaction(), Encoding.UTF8, RequestType);
            var response = await client.PutAsync(Put.UpdateTransaction,content);
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Post_Add_Transaction_response_ok_status_code()
        {
            using var client = _thisServer.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
            var content = new StringContent(BuildTransaction(), Encoding.UTF8, RequestType);
            var response = await client.PostAsync(Post.AddTransaction, content);
            response.EnsureSuccessStatusCode();
        }
        private async Task<string> GetToken()
        {
            using var client = _testServerIdentity.CreateClient();
            var content = new StringContent(BuildUser(), Encoding.UTF8, RequestType);
            await client.PostAsync(IdentityScenariosBase.Post.Register, content);
            var response = await client.PostAsync(IdentityScenariosBase.Post.Login, content);
            var contentString = await response.Content.ReadAsStringAsync();
            var userInfo = DeserializeResponse<ApiResult<AuthenticateResponse>>(contentString).Result;
            return userInfo.Token;
        }
        private static string BuildUser()
        {
            var user = new RegisterRequest
            {
                Username = "admin",
                Password = "Pass1234"
            };
            return JsonSerializer.Serialize(user);
        }
        private static string BuildTransaction()
        {
            var transaction = new Transaction
            {
                Id = 3,
                Products = new List<Product>
                {
                    new Product()
                    {
                        Name = "string",
                        ProductId = 100,
                        Count = 1,
                        Cost = 123,
                        Category = "самса"
                    }
                },
                Date = DateTime.Now,
                TransactionType = TransactionTypes.InCash,
                IsShopCreate = false
            };
            return JsonSerializer.Serialize(transaction);
        }
        private static string BuildUpdateTransaction()
        {
            var transaction = new UpdateTransaction()
            {
                Id = 1,
                TransactionType = TransactionTypes.ByCard,
            };
            return JsonSerializer.Serialize(transaction);
        }

        private static T DeserializeResponse<T>(string content)
            => JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        ~PurchasesScenarios()
        {
            _testServerIdentity.Dispose();
            _thisServer.Dispose();
        }
    }
}
