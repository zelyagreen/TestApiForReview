﻿using TestApiForReview.Infrastructure.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace TestApiForReview.Infrastructure.Models.Shops
{
    [ProductValidation]
    public class Product
    {
        public string Name { get; set; }
        public int ProductId { get; set; }
        public decimal Cost { get; set; }
        public int Count { get; set; }
        public string Category { get; set; }
    }
    public class ProductValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var employee = (Product)value;
            if (employee.ProductId<1)
                throw new BadRequestException($"Invalid ProductId: ProductId can't be {employee.ProductId}");
            if (employee.Count<1)
                throw new BadRequestException($"Invalid Count: Product count can't be {employee.Count}");
            return ValidationResult.Success;
        }
    }
}
