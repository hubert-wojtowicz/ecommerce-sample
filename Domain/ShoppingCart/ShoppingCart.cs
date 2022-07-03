using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Common.Domain;
using Common.Domain.User.ValueObjects;
using Domain.Common;
using Domain.DiscountVoucher.ValueObjects;
using Domain.Product;
using Domain.Product.ValueObjects;
using Domain.ShoppingCart.ValueObjects;

namespace Domain.ShoppingCart
{
    public class ShoppingCart : AggregateRoot<ShoppingCartId>
    {
        public UserId UserId { get; }
        public DiscountVoucherId DiscountVoucherId { get; private set; }
        private readonly ICollection<ShoppingCartItem> _items = new Collection<ShoppingCartItem>();
        public bool IsEmpty => _items.Any();

        public ShoppingCart(UserId userId)
        {
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
        }

        public void AddProduct(ProductId productId)
        {
            var item = _items.FirstOrDefault(item => item.ProductId.Equals(productId));
            if (item is not null)
            {
                item.IncreaseAmount();
            }
            else
            {
                _items.Add(new ShoppingCartItem(productId));
            }
        }

        public void UseDiscountCode(DiscountVoucherId discountVoucherId)
        {
            if (DiscountVoucherId is not null)
            {
                throw new ApplicationException("Can't use second discount code.");
            }
            DiscountVoucherId = discountVoucherId ?? throw new ArgumentNullException(nameof(discountVoucherId));
        }

        public Task<Money> GetCartTotalValue(Dictionary<ProductId, Money> prices)
        {
            decimal total = _items.Sum(x => x.Amount.Value * prices[x.ProductId].Value);

            return Task.FromResult(new Money(total));
        }

        public async Task<Dictionary<ProductId, Money>> GetProductPrices(IProductRepository productRepository)
        {
            // assune all productId validated and exist
            var productIds = _items.Select(x => x.ProductId).ToList();
            return await productRepository.GetAllPrices(productIds);
        }

        public Task<Money> GetDiscountValue(DiscountVoucher.DiscountVoucher discountVoucher)
        {
            if (discountVoucher.IsExpired()) return Task.FromResult(Money.Zero);

            return Task.FromResult(discountVoucher.Value);
        }
    }
}
