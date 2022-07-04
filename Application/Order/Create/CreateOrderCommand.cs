using Domain.Common;
using MediatR;
using System;

namespace Application.Order.Create
{
    public class CreateOrderCommand : IRequest
    {
        public Guid ShoppingCartId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string Remarks { get; set; }
        
        public CreateOrderCommand(Guid shoppingCartId, PaymentMethod paymentMethod, string remarks)
        {
            ShoppingCartId = shoppingCartId;
            PaymentMethod = paymentMethod;
            Remarks = remarks;
        }
    }
}
