using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Common;
using Domain.Product;
using Domain.Product.ValueObjects;

namespace Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product, ProductId>, IProductRepository
    {
        public ProductRepository()
        {
            Collection.Add(new Product(ProductId.CreateOrNull(Guid.Parse("1106c741-22e7-4038-899d-aef4449bfea8")), new Money(10)));
            Collection.Add(new Product(ProductId.CreateOrNull(Guid.Parse("88a7f65f-a2a8-401c-a804-3fd7bde7f24f")), new Money(20)));
            Collection.Add(new Product(ProductId.CreateOrNull(Guid.Parse("917351d7-af28-4caf-9ec9-fe7e5d281554")), new Money(30)));
        }

        public async Task<Dictionary<ProductId, Money>> GetAllPrices(ICollection<ProductId> productIds)
        {
            // I decided to put so to show this is in one optimized query on db side
            return productIds.ToDictionary(productId => productId, productId => Collection.Single(product => product.Id == productId).Price);
        }
    }
}
