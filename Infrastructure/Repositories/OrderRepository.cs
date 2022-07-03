using Domain.Order;
using Domain.Order.ValueObjects;

namespace Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order, OrderId>, IOrderRepository { }
}
