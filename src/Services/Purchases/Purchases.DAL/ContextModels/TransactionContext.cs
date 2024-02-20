﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestApiForReview.Infrastructure.Models.Purchases;

namespace Purchases.DAL.ContextModels
{
    public class TransactionContext 
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("CustomerContextKey")]
        public int CustomerContextKey { get; set; }
        public List<ProductContext> Products { get; set; } = new List<ProductContext>();
        public DateTime Date { get; set; }
        public TransactionTypes TransactionType { get; set; } = TransactionTypes.InCash;
        public bool IsShopCreate { get; set; }
        public ReceiptContext Receipt { get; set; }
    }
}
