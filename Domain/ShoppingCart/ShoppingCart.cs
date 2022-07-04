using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Common.Domain;
using Common.Domain.User.ValueObjects;
using Domain.Common;
using Domain.DiscountVoucher.ValueObjects;
using Domain.Product.ValueObjects;
using Domain.ShoppingCart.ValueObjects;

namespace Domain.ShoppingCart
{
    public class ShoppingCart : AggregateRoot<ShoppingCartId>
    {
        public UserId UserId { get; }
        public DiscountVoucherId DiscountVoucherId { get; private set; }
        private ICollection<ShoppingCartItem> _items = new Collection<ShoppingCartItem>();
        public bool IsShoppingCartEmpty => !_items.Any();

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

        public Dictionary<ProductId, Amount> GetCartProductsAmounts()
        {
            return _items.ToDictionary(x => x.ProductId, y => y.Amount);
        }

        public void UseDiscountCode(DiscountVoucherId discountVoucherId)
        {
            if (DiscountVoucherId is not null)
            {
                throw new ApplicationException("Can't use second discount code.");
            }
            DiscountVoucherId = discountVoucherId ?? throw new ArgumentNullException(nameof(discountVoucherId));
        }

        public void CleanCart()
        {
            DiscountVoucherId = null;
            _items = new Collection<ShoppingCartItem>();
        }
    }
}
