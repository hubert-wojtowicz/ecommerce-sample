using Common.Domain.ActionContext;
using Domain.DiscountVoucher;
using Domain.Order;
using Domain.Product;
using Domain.ShoppingCart;
using Domain.ShoppingCartSnapshot;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Order.Create
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand>
    {
        private readonly IShoppingCartSnapshotRepository _shoppingCartSnapshotRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IDiscountVoucherRepository _discountVoucherRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IActionContextProvider _actionContextProvider;
        private readonly IProductRepository _productRepository;

        public CreateOrderCommandHandler(
            IShoppingCartSnapshotRepository shoppingCartSnapshotRepository,
            IOrderRepository orderRepository,
            IDiscountVoucherRepository discountVoucherRepository,
            IShoppingCartRepository shoppingCartRepository,
            IActionContextProvider actionContextProvider,
            IProductRepository productRepository)
        {
            _shoppingCartSnapshotRepository = shoppingCartSnapshotRepository;
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
            ShoppingCartSnapshot shoppingCartSnapshot = new();
            await shoppingCartSnapshot.PopulateCartSnapshot(shoppingCart, _productRepository);
            await _shoppingCartSnapshotRepository.InsertAsync(shoppingCartSnapshot);

            var order = new Domain.Order.Order();
            order.SetPaymentMethod(command.PaymentMethod);
            order.SetRemarks(command.Remarks);

            // start transaction scope
            await order.Calculate(shoppingCartSnapshot, _productRepository, _discountVoucherRepository);
            var orderNumber = await _orderRepository.GetNextOrderNumber();
            order.SetOrderNumber(orderNumber);
            await _orderRepository.InsertAsync(order);
            shoppingCart.CleanCart();
            //TODO: _discountVoucherRepository.Update(voucher);

            // end transaction

            return Unit.Value;
        }
    }
}
