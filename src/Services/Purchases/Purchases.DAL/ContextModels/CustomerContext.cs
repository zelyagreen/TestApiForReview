﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Purchases.DAL.ContextModels
{
    public class CustomerContext
    {
        [Key]
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public List<TransactionContext> Transactions { get; set; } = new List<TransactionContext>();
    }
}
