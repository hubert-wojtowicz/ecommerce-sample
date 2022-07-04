using Common.Domain.ValueObjects.Money;

namespace Domain.DiscountVoucher.ValueObjects
{
    public class Value : MoneyValueObject
    {
        public Value() : base()
        {
        }

        public Value(decimal value) : base(value)
        {
        }

        public static Value CreateOrNull(decimal? value) => value is null ? null : new Value(value.Value);
    }
}