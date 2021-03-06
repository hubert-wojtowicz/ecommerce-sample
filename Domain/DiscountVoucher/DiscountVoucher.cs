using System;
using Common.Domain;
using Domain.Common;
using Domain.DiscountVoucher.ValueObjects;

namespace Domain.DiscountVoucher
{
    public class DiscountVoucher : AggregateRoot<DiscountVoucherId>
    {
        public ExpirationDate ExpirationDate { get; }
        public Code Code { get; }
        public Money Value { get; }
        public bool IsUsed { get; private set; } = false;

        public bool IsValid()
        {
            return !IsExpired() && !IsUsed ;
        }

        public bool IsExpired()
        {
            var exp = ExpirationDate.Value;
            var now = DateTime.UtcNow;
            return exp < now;
        }

        public DiscountVoucher(ExpirationDate expirationDate, Code code, Money value) : base(new DiscountVoucherId())
        {
            ExpirationDate = expirationDate ?? throw new ArgumentNullException(nameof(expirationDate));
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public void ApplyDiscount()
        {
            IsUsed = true;
        }
    }
}
