using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Order.Create
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand>
    {
        public CreateOrderCommandHandler()
        {

        }

        public Task<Unit> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
