using Common.Domain;
using Domain.Common;
using Domain.Order.ValueObjects;
using Domain.Product.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Order
{
    public class Order : AggregateRoot<OrderId>
    {
        public Money TotalValue { get; private set; }

        public Money Discount { get; private set; }

        public ShoppingCart.ShoppingCart ShoppingCart { get; private set; }

        public async Task<OrderId> CreateOrder(
            ShoppingCart.ShoppingCart shoppingCart,
            Dictionary<ProductId, Money> prices,
            DiscountVoucher.DiscountVoucher discountVoucher)
        {
            if (!shoppingCart.IsEmpty) throw new ApplicationException("For creating an order cart can not be empty.");
            Id = new OrderId(Guid.NewGuid());
            ShoppingCart = shoppingCart;
            TotalValue = await shoppingCart.GetCartTotalValue(prices);
            Discount = await shoppingCart.GetDiscountValue(discountVoucher);
            return Id;
        }
    }
}
