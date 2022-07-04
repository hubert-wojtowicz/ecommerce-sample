using Common.Domain;
using Domain.Common;
using Domain.DiscountVoucher;
using Domain.Order.ValueObjects;
using Domain.Product;
using Domain.ShoppingCartSnapshot.ValueObjects;
using System;
using System.Threading.Tasks;

namespace Domain.Order
{
    public class Order : AggregateRoot<OrderId>
    {
        public int OrderNumber { get; private set; }

        public PaymentMethod PaymentMethod { get; private set; }

        public string Remarks { get; private set; }

        public Money ItemLinesTotalValue => _shoppingCartSnapshot.GetCartTotalValue();

        public Money DiscountValue { get; private set; }

        public ShoppingCartSnapshotId ShoppingCartSnapshotId { get; private set; }

        private ShoppingCartSnapshot.ShoppingCartSnapshot _shoppingCartSnapshot;

        public void SetOrderNumber(int orderNumber)
        {
            OrderNumber = orderNumber;
        }

        public void SetPaymentMethod(PaymentMethod paymentMethod)
        {
            PaymentMethod = paymentMethod;
        }

        public void SetRemarks(string remarks)
        {
            if (remarks != null && remarks.Length > 200) throw new ApplicationException("Remarks must be null or max character length of 200.");

            Remarks = remarks;
        }

        public async Task Calculate(ShoppingCartSnapshot.ShoppingCartSnapshot cartSnapshot, IProductRepository productRepository, IDiscountVoucherRepository discountRepository)
        {
            Id = new OrderId(Guid.NewGuid());
            ShoppingCartSnapshotId = cartSnapshot.Id;
            _shoppingCartSnapshot = cartSnapshot;

            var discountExist = await discountRepository.ExistsAsync(_shoppingCartSnapshot.DiscountVoucherId);
            if (!discountExist)
                DiscountValue = Money.Zero;
            else
            {
                var voucher = await discountRepository.GetAsync(_shoppingCartSnapshot.DiscountVoucherId);
                await ApplyDiscountVoucher(voucher);
            }
        }

        private Task ApplyDiscountVoucher(DiscountVoucher.DiscountVoucher discountVoucher)
        {
            if (!discountVoucher.IsValid())
            {
                DiscountValue = Money.Zero;
            }
            else
            {
                discountVoucher.ApplyDiscount();
                DiscountValue = discountVoucher.Value;
            }

            return Task.CompletedTask;
        }
    }
}
