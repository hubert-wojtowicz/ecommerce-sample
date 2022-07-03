using System;

namespace WebApi.Controllers.Order
{
    public class CreateOrderRequestModel
    {
        public Guid ShoppingCartId { get; set; }
        public string PaymentMethod { get; set; }
        public string Remarks { get; set; }
    }
}
