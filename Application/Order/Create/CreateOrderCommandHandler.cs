using Common.Domain.ActionContext;
using Domain.Common;
using Domain.DiscountVoucher;
using Domain.Order;
using Domain.Product;
using Domain.Product.ValueObjects;
using Domain.ShoppingCart;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Order.Create
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IDiscountVoucherRepository _discountVoucherRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IActionContextProvider _actionContextProvider;
        private readonly IProductRepository _productRepository;

        public CreateOrderCommandHandler(
            IOrderRepository orderRepository,
            IDiscountVoucherRepository discountVoucherRepository,
            IShoppingCartRepository shoppingCartRepository,
            IActionContextProvider actionContextProvider,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _discountVoucherRepository = discountVoucherRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _actionContextProvider = actionContextProvider;
            _productRepository = productRepository;
        }

        public async Task<Unit> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var shoppingCart = await _shoppingCartRepository.GetByUserIdAsync(_actionContextProvider.ActionContext.UserId);
            Dictionary<ProductId, Money> prices =  await shoppingCart.GetProductPrices(_productRepository);
            var voucher = await _discountVoucherRepository.GetAsync(shoppingCart.DiscountVoucherId);

            cancellationToken.ThrowIfCancellationRequested();

            var order = new Domain.Order.Order();
            await order.CreateOrder(shoppingCart, prices, voucher);
            await _orderRepository.InsertAsync(order);
            await _discountVoucherRepository.DeleteAsync(voucher);
            return Unit.Value;
        }
    }
}
