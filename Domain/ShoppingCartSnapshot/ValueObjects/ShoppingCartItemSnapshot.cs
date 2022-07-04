using Common.Domain.ValueObjects;
using Domain.Common;
using System.Collections.Generic;

namespace Domain.ShoppingCartSnapshot.ValueObjects
{
    public class ShoppingCartItemSnapshot : ValueObject
    {
        public Amount Amount { get; private set; }
        public Money Money { get; private set; }

        public ShoppingCartItemSnapshot(Amount amount, Money money)
        {
            Amount = amount;
            Money = money;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            return new object[] { Amount.Value, Money.Value };
        }
    }
}
