using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Domain;
using Domain.Common;
using Domain.Product.ValueObjects;

namespace Domain.Product
{
    public interface IProductRepository : IRepository<Product, ProductId>
    {
        Task<Dictionary<ProductId, Money>> GetAllPrices(ICollection<ProductId> productIds);
    }
}
