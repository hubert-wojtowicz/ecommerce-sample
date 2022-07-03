using Common.Domain.ValueObjects.Money;

namespace Domain.Common
{
    public class Money : MoneyValueObject
    {
        public Money(decimal value) : base(value, 0m, 99999999999.99m)
        {
        }

        public static Money Zero => new(0);
    }
}
