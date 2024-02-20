﻿using System.Collections.Generic;
using System.Threading.Tasks;
using TestApiForReview.Infrastructure.MassTransit;
using TestApiForReview.Infrastructure.Models.Purchases;
using TestApiForReview.Infrastructure.Models.Shops;

namespace Shops.Domain.Services
{
    public interface IShopsService
    {
        public ICollection<Shop> GetAllShops();
        public Task<ICollection<Product>> GetProductsByShop(int shopId);
        public Task<ICollection<Product>> GetProductsByCategory(int shopId, string categoryName);
        public Task<ICollection<Product>> BuyProducts(int shopId, ICollection<Product> products);
        public Task AddProductsByFactory(ICollection<ProductByFactory> products);
        public Task<Transaction> CreateTransaction(int shopId, ICollection<Product> products);
        public Task AddReceipt(Receipt receipt);
    }
}
