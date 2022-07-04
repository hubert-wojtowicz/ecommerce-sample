using Common.Domain;
using Domain.Common;
using Domain.DiscountVoucher.ValueObjects;
using Domain.Product;
using Domain.Product.ValueObjects;
using Domain.ShoppingCartSnapshot.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ShoppingCartSnapshot
{
    public class ShoppingCartSnapshot : AggregateRoot<ShoppingCartSnapshotId>
    {
        public DiscountVoucherId DiscountVoucherId { get; private set; }
        private Dictionary<ProductId, ShoppingCartItemSnapshot> _cartItems;

        public async Task PopulateCartSnapshot(ShoppingCart.ShoppingCart shoppingCart, IProductRepository productRepository)
        {
            if (shoppingCart.IsShoppingCartEmpty) throw new ApplicationException("Cart can not be empty.");

            DiscountVoucherId = shoppingCart.DiscountVoucherId;
            _cartItems = new();

            // assune all productId validated and exist 
            var productsAmounts = shoppingCart.GetCartProductsAmounts();
            var productPrices = await productRepository.GetAllPrices(productsAmounts.Keys);
            _cartItems = productsAmounts.ToDictionary(x => x.Key, y => new ShoppingCartItemSnapshot(y.Value, productPrices[y.Key]));
        }

        public Money GetCartTotalValue()
        {
            decimal total = _cartItems.Sum(x => x.Value.Amount.Value * x.Value.Money.Value);
            return new Money(total);
        }
    }
}
