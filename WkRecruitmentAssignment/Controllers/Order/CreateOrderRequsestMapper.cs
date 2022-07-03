using Application.Order.Create;
using System;

namespace WebApi.Controllers.Order
{
    public class CreateOrderRequsestMapper : ICreateOrderRequsestMapper
    {
        public CreateOrderCommand Map(CreateOrderRequestModel request)
        {
            var paymentMethod = Enum.Parse<PaymentMethod>(request.PaymentMethod);

            return new CreateOrderCommand(
                request.ShoppingCartId,
                paymentMethod,
                request.Remarks);
        }
    }

    public interface ICreateOrderRequsestMapper
    {
        CreateOrderCommand Map(CreateOrderRequestModel request);
    }
}
