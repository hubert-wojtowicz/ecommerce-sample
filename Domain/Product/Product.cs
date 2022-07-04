using System;
using Common.Domain;
using Domain.Common;
using Domain.Product.ValueObjects;

namespace Domain.Product
{
    public class Product : AggregateRoot<ProductId>
    {
        public Money Price { get; }

        public Product(ProductId productId, Money price) : base(productId)
        {
            Price = price ?? throw new ArgumentNullException(nameof(price));
        }

    }
}
