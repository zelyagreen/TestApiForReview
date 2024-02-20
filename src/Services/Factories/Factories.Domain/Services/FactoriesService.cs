﻿using Factories.DAL.Data;
using Factories.Domain.Helpers;
using Microsoft.EntityFrameworkCore;
using TestApiForReview.Infrastructure.Models.Shops;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Factories.Domain.Services
{
    public class FactoriesService : IFactoriesService
    {
        private readonly FactoriesDbContext _context;
        public FactoriesService(FactoriesDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<ProductByFactory>> CreateRequestInShops()
        => await UpdateFactoriesStage();
        private async Task<ICollection<ProductByFactory>> UpdateFactoriesStage()
        {
            var result = new List<ProductByFactory>();
            await _context.Factories.Include(item => item.Products).ForEachAsync(factory =>
            {
                factory.Products.ForEach(product =>
                {
                    product.Count += product.PartsOfCreate;
                    if (product.Count < 1) return;
                    result.Add(product.ToProductByFactoryDto());
                    product.Count -= (int) product.Count;
                });
            });
            await _context.SaveChangesAsync();
            return result;
        }
    }
}
